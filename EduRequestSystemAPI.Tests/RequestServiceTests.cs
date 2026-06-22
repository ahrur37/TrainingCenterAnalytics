using EduRequestSystemAPI.DatabaseContext;
using EduRequestSystemAPI.Enums;
using EduRequestSystemAPI.Models;
using EduRequestSystemAPI.Requests;
using EduRequestSystemAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace EduRequestSystemAPI.Tests;

/// <summary>
/// Тесты сервиса заявок: создание заявки и фильтрация/поиск.
/// </summary>
public class RequestServiceTests
{
    // ──────────────────────────── Тест №3: Создание заявки ─────────────────────────

    [Fact]
    public async Task CreateRequestAsync_PersistsRequestWithNewStatusAndAuditLog()
    {
        // Arrange
        await using var ctx = TestHelpers.CreateInMemoryContext();
        var service = new RequestService(ctx);

        var model = new CreateRequest
        {
            Topic = "Курс по C#",
            Description = "Нужно обучить команду основам .NET",
            ContactInfo = "tel:+70000000000",
            DirectionId = 1,
            TrainingFormatId = 1,
            AuthorId = 42
        };

        // Act
        var result = await service.CreateRequestAsync(model);

        // Assert
        Assert.IsType<OkObjectResult>(result);

        var request = Assert.Single(ctx.Requests);
        Assert.Equal("Курс по C#", request.Topic);
        Assert.Equal(42, request.AuthorId);
        Assert.Equal(1, request.StatusId);                 // новая заявка получает статус 1 ("Новая")
        Assert.True(request.CreatedAt > DateTime.MinValue); // дата создания проставлена

        // Создание заявки фиксируется в журнале аудита и ссылается на её Id
        Assert.Contains(ctx.AuditLogs,
            a => a.Action == AuditAction.CreateRequest && a.EntityId == request.Id);
    }

    // ───────────────────────── Тест №5: Фильтрация заявок ──────────────────────────

    [Fact]
    public async Task GetRequestsAsync_WithSearchTerm_ReturnsOnlyMatchingByTopicOrDescription()
    {
        // Arrange
        await using var ctx = TestHelpers.CreateInMemoryContext();
        await SeedRequestsAsync(ctx);
        var service = new RequestService(ctx);

        // Act — регистр не должен иметь значения ("КУРС" найдёт "Курс по ...")
        var result = await service.GetRequestsAsync(
            statusId: null, directionId: null, searchTerm: "КУРС", assigneeId: null, authorId: null);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        var requests = Assert.IsType<List<Request>>(ok.Value);

        Assert.Equal(2, requests.Count); // обе заявки со словом "Курс" в теме
        Assert.All(requests, r => Assert.Contains("курс", r.Topic.ToLower()));
    }

    [Fact]
    public async Task GetRequestsAsync_WithStatusFilter_ReturnsOnlyRequestsWithThatStatus()
    {
        // Arrange
        await using var ctx = TestHelpers.CreateInMemoryContext();
        await SeedRequestsAsync(ctx);
        var service = new RequestService(ctx);

        // Act — только заявки со статусом 1 ("Новая")
        var result = await service.GetRequestsAsync(
            statusId: 1, directionId: null, searchTerm: null, assigneeId: null, authorId: null);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        var requests = Assert.IsType<List<Request>>(ok.Value);

        Assert.Equal(2, requests.Count);
        Assert.All(requests, r => Assert.Equal(1, r.StatusId));
    }

    /// <summary>
    /// Три заявки для проверки фильтров:
    /// две со словом "Курс" в теме и статусом 1, одна — статус 2.
    /// GetRequestsAsync делает .Include() по обязательным связям (направление, статус,
    /// формат, автор), поэтому связанные справочные записи тоже нужно создать —
    /// иначе INNER JOIN отфильтрует заявки.
    /// </summary>
    private static async Task SeedRequestsAsync(ContextDb ctx)
    {
        ctx.Directions.AddRange(
            new Direction { Id = 1, Name = "Программирование" },
            new Direction { Id = 2, Name = "Дизайн" });
        ctx.Statuses.AddRange(
            new Status { Id = 1, Name = "Новая" },
            new Status { Id = 2, Name = "В работе" });
        ctx.TrainingFormats.AddRange(
            new TrainingFormat { Id = 1, Name = "Онлайн" },
            new TrainingFormat { Id = 2, Name = "Очно" });
        ctx.Users.AddRange(
            new User { Id = 10, Name = "Автор 1", Email = "author1@test.com", Password = "secret123", RoleId = 1 },
            new User { Id = 11, Name = "Автор 2", Email = "author2@test.com", Password = "secret123", RoleId = 1 });

        ctx.Requests.AddRange(
            new Request
            {
                Topic = "Курс по Python", Description = "Основы языка",
                StatusId = 1, DirectionId = 1, TrainingFormatId = 1, AuthorId = 10,
                CreatedAt = DateTime.UtcNow
            },
            new Request
            {
                Topic = "Курс по C#", Description = "ООП и паттерны",
                StatusId = 2, DirectionId = 1, TrainingFormatId = 1, AuthorId = 10,
                CreatedAt = DateTime.UtcNow
            },
            new Request
            {
                Topic = "Дизайн интерфейсов", Description = "Работа в Figma",
                StatusId = 1, DirectionId = 2, TrainingFormatId = 2, AuthorId = 11,
                CreatedAt = DateTime.UtcNow
            });

        await ctx.SaveChangesAsync();
    }
}

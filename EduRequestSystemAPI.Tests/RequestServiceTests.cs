using EduRequestSystemAPI.DatabaseContext;
using EduRequestSystemAPI.Enums;
using EduRequestSystemAPI.Models;
using EduRequestSystemAPI.Requests;
using EduRequestSystemAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace EduRequestSystemAPI.Tests;

public class RequestServiceTests
{
    [Fact]
    public async Task CreateRequestAsync_PersistsRequestWithNewStatusAndAuditLog()
    {
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

        var result = await service.CreateRequestAsync(model);

        Assert.IsType<OkObjectResult>(result);

        var request = Assert.Single(ctx.Requests);
        Assert.Equal("Курс по C#", request.Topic);
        Assert.Equal(42, request.AuthorId);
        Assert.Equal(1, request.StatusId);
        Assert.True(request.CreatedAt > DateTime.MinValue);

        Assert.Contains(ctx.AuditLogs,
            a => a.Action == AuditAction.CreateRequest && a.EntityId == request.Id);
    }

    [Fact]
    public async Task GetRequestsAsync_WithSearchTerm_ReturnsOnlyMatchingByTopicOrDescription()
    {
        await using var ctx = TestHelpers.CreateInMemoryContext();
        await SeedRequestsAsync(ctx);
        var service = new RequestService(ctx);

        var result = await service.GetRequestsAsync(
            statusId: null, directionId: null, searchTerm: "КУРС", assigneeId: null, authorId: null);

        var ok = Assert.IsType<OkObjectResult>(result);
        var requests = Assert.IsType<List<Request>>(ok.Value);

        Assert.Equal(2, requests.Count);
        Assert.All(requests, r => Assert.Contains("курс", r.Topic.ToLower()));
    }

    [Fact]
    public async Task GetRequestsAsync_WithStatusFilter_ReturnsOnlyRequestsWithThatStatus()
    {
        await using var ctx = TestHelpers.CreateInMemoryContext();
        await SeedRequestsAsync(ctx);
        var service = new RequestService(ctx);

        var result = await service.GetRequestsAsync(
            statusId: 1, directionId: null, searchTerm: null, assigneeId: null, authorId: null);

        var ok = Assert.IsType<OkObjectResult>(result);
        var requests = Assert.IsType<List<Request>>(ok.Value);

        Assert.Equal(2, requests.Count);
        Assert.All(requests, r => Assert.Equal(1, r.StatusId));
    }

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
                Topic = "Курс по Python", Description = "Основы языка", ContactInfo = "py@test.com",
                StatusId = 1, DirectionId = 1, TrainingFormatId = 1, AuthorId = 10,
                CreatedAt = DateTime.UtcNow
            },
            new Request
            {
                Topic = "Курс по C#", Description = "ООП и паттерны", ContactInfo = "csharp@test.com",
                StatusId = 2, DirectionId = 1, TrainingFormatId = 1, AuthorId = 10,
                CreatedAt = DateTime.UtcNow
            },
            new Request
            {
                Topic = "Дизайн интерфейсов", Description = "Работа в Figma", ContactInfo = "design@test.com",
                StatusId = 1, DirectionId = 2, TrainingFormatId = 2, AuthorId = 11,
                CreatedAt = DateTime.UtcNow
            });

        await ctx.SaveChangesAsync();
    }
}

using EduRequestSystemAPI.Enums;
using EduRequestSystemAPI.Models;
using EduRequestSystemAPI.Requests;
using EduRequestSystemAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace EduRequestSystemAPI.Tests;

/// <summary>
/// Тесты сервиса пользователей: регистрация (валидация) и авторизация (создание сессии).
/// </summary>
public class UserServiceTests
{
    // ───────────────────────────── Тест №1: Регистрация ─────────────────────────────

    [Fact]
    public async Task RegUserAsync_WithShortPassword_ReturnsBadRequestAndSavesNothing()
    {
        // Arrange
        await using var ctx = TestHelpers.CreateInMemoryContext();
        var service = new UserService(ctx, TestHelpers.CreateJwtGenerator());

        var model = new RegUserModel { Name = "Иван", Email = "ivan@test.com", Password = "123" };

        // Act — пароль короче 6 символов
        var result = await service.RegUserAsync(model);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        Assert.Empty(ctx.Users); // пользователь не должен быть создан
    }

    [Fact]
    public async Task RegUserAsync_WithDuplicateEmail_ReturnsBadRequest()
    {
        // Arrange — пользователь с таким email уже есть в БД
        await using var ctx = TestHelpers.CreateInMemoryContext();
        ctx.Users.Add(new User { Name = "Старый", Email = "ivan@test.com", Password = "secret123", RoleId = 1 });
        await ctx.SaveChangesAsync();

        var service = new UserService(ctx, TestHelpers.CreateJwtGenerator());

        // Act — тот же email в другом регистре (проверяем регистронезависимость)
        var result = await service.RegUserAsync(
            new RegUserModel { Name = "Иван", Email = "IVAN@test.com", Password = "secret123" });

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        Assert.Single(ctx.Users); // дубликат не добавлен
    }

    [Fact]
    public async Task RegUserAsync_WithValidData_CreatesUserWithApplicantRole()
    {
        // Arrange
        await using var ctx = TestHelpers.CreateInMemoryContext();
        var service = new UserService(ctx, TestHelpers.CreateJwtGenerator());

        var model = new RegUserModel { Name = "Иван", Email = "ivan@test.com", Password = "secret123" };

        // Act
        var result = await service.RegUserAsync(model);

        // Assert
        Assert.IsType<OkObjectResult>(result);

        var user = Assert.Single(ctx.Users);
        Assert.Equal("ivan@test.com", user.Email);
        Assert.Equal("Иван", user.Name);
        Assert.Equal(1, user.RoleId); // новым пользователям назначается роль 1 (Заявитель)

        // Регистрация фиксируется в журнале аудита
        Assert.Contains(ctx.AuditLogs, a => a.Action == AuditAction.Register);
    }

    // ──────────────────────────── Тест №2: Авторизация ─────────────────────────────

    [Fact]
    public async Task AuthUserAsync_WithValidCredentials_ReturnsOkAndCreatesSession()
    {
        // Arrange — заранее зарегистрированный пользователь
        await using var ctx = TestHelpers.CreateInMemoryContext();
        var user = new User { Name = "Иван", Email = "ivan@test.com", Password = "secret123", RoleId = 2 };
        ctx.Users.Add(user);
        await ctx.SaveChangesAsync();

        var service = new UserService(ctx, TestHelpers.CreateJwtGenerator());

        // Act
        var result = await service.AuthUserAsync(
            new AuthUserModel { Email = "ivan@test.com", Password = "secret123" });

        // Assert
        Assert.IsType<OkObjectResult>(result);

        // Ключевой момент кастомной авторизации: в таблице Sessions появляется токен
        var session = Assert.Single(ctx.Sessions);
        Assert.False(string.IsNullOrEmpty(session.Token));
        Assert.Equal(user.Id, session.UserId);

        // Вход фиксируется в журнале аудита
        Assert.Contains(ctx.AuditLogs, a => a.Action == AuditAction.Login);
    }

    [Fact]
    public async Task AuthUserAsync_WithWrongPassword_ReturnsNotFoundAndCreatesNoSession()
    {
        // Arrange
        await using var ctx = TestHelpers.CreateInMemoryContext();
        ctx.Users.Add(new User { Name = "Иван", Email = "ivan@test.com", Password = "secret123", RoleId = 2 });
        await ctx.SaveChangesAsync();

        var service = new UserService(ctx, TestHelpers.CreateJwtGenerator());

        // Act — неверный пароль
        var result = await service.AuthUserAsync(
            new AuthUserModel { Email = "ivan@test.com", Password = "wrong-password" });

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
        Assert.Empty(ctx.Sessions); // сессия не должна создаваться
    }
}

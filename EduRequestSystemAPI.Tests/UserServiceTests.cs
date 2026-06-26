using EduRequestSystemAPI.Enums;
using EduRequestSystemAPI.Models;
using EduRequestSystemAPI.Requests;
using EduRequestSystemAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace EduRequestSystemAPI.Tests;

public class UserServiceTests
{
    [Fact]
    public async Task RegUserAsync_WithShortPassword_ReturnsBadRequestAndSavesNothing()
    {
        await using var ctx = TestHelpers.CreateInMemoryContext();
        var service = new UserService(ctx, TestHelpers.CreateJwtGenerator());

        var model = new RegUserModel { Name = "Иван", Email = "ivan@test.com", Password = "123" };

        var result = await service.RegUserAsync(model);

        Assert.IsType<BadRequestObjectResult>(result);
        Assert.Empty(ctx.Users);
    }

    [Fact]
    public async Task RegUserAsync_WithDuplicateEmail_ReturnsBadRequest()
    {
        await using var ctx = TestHelpers.CreateInMemoryContext();
        ctx.Users.Add(new User { Name = "Старый", Email = "ivan@test.com", Password = "secret123", RoleId = 1 });
        await ctx.SaveChangesAsync();

        var service = new UserService(ctx, TestHelpers.CreateJwtGenerator());

        var result = await service.RegUserAsync(
            new RegUserModel { Name = "Иван", Email = "IVAN@test.com", Password = "secret123" });

        Assert.IsType<BadRequestObjectResult>(result);
        Assert.Single(ctx.Users);
    }

    [Fact]
    public async Task RegUserAsync_WithValidData_CreatesUserWithApplicantRole()
    {
        await using var ctx = TestHelpers.CreateInMemoryContext();
        var service = new UserService(ctx, TestHelpers.CreateJwtGenerator());

        var model = new RegUserModel { Name = "Иван", Email = "ivan@test.com", Password = "secret123" };

        var result = await service.RegUserAsync(model);

        Assert.IsType<OkObjectResult>(result);

        var user = Assert.Single(ctx.Users);
        Assert.Equal("ivan@test.com", user.Email);
        Assert.Equal("Иван", user.Name);
        Assert.Equal(1, user.RoleId);

        Assert.Contains(ctx.AuditLogs, a => a.Action == AuditAction.Register);
    }

    [Fact]
    public async Task AuthUserAsync_WithValidCredentials_ReturnsOkAndCreatesSession()
    {
        await using var ctx = TestHelpers.CreateInMemoryContext();
        var user = new User { Name = "Иван", Email = "ivan@test.com", Password = "secret123", RoleId = 2 };
        ctx.Users.Add(user);
        await ctx.SaveChangesAsync();

        var service = new UserService(ctx, TestHelpers.CreateJwtGenerator());

        var result = await service.AuthUserAsync(
            new AuthUserModel { Email = "ivan@test.com", Password = "secret123" });

        Assert.IsType<OkObjectResult>(result);

        var session = Assert.Single(ctx.Sessions);
        Assert.False(string.IsNullOrEmpty(session.Token));
        Assert.Equal(user.Id, session.UserId);

        Assert.Contains(ctx.AuditLogs, a => a.Action == AuditAction.Login);
    }

    [Fact]
    public async Task AuthUserAsync_WithWrongPassword_ReturnsNotFoundAndCreatesNoSession()
    {
        await using var ctx = TestHelpers.CreateInMemoryContext();
        ctx.Users.Add(new User { Name = "Иван", Email = "ivan@test.com", Password = "secret123", RoleId = 2 });
        await ctx.SaveChangesAsync();

        var service = new UserService(ctx, TestHelpers.CreateJwtGenerator());

        var result = await service.AuthUserAsync(
            new AuthUserModel { Email = "ivan@test.com", Password = "wrong-password" });

        Assert.IsType<NotFoundObjectResult>(result);
        Assert.Empty(ctx.Sessions);
    }
}

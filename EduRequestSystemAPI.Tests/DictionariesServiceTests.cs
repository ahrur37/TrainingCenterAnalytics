using EduRequestSystemAPI.Models;
using EduRequestSystemAPI.Requests;
using EduRequestSystemAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace EduRequestSystemAPI.Tests;

/// <summary>
/// Тесты сервиса справочников на примере создания направления обучения.
/// </summary>
public class DictionariesServiceTests
{
    // ─────────────────────── Тест №4: Создание направления ──────────────────────────

    [Fact]
    public async Task CreateDirectionAsync_WithEmptyName_ReturnsBadRequestAndSavesNothing()
    {
        // Arrange
        await using var ctx = TestHelpers.CreateInMemoryContext();
        var service = new DictionariesService(ctx);

        // Act — пустое/пробельное название недопустимо
        var result = await service.CreateDirectionAsync(new DictionaryModel { Name = "   " });

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        Assert.Empty(ctx.Directions);
    }

    [Fact]
    public async Task CreateDirectionAsync_WithValidName_PersistsDirection()
    {
        // Arrange
        await using var ctx = TestHelpers.CreateInMemoryContext();
        var service = new DictionariesService(ctx);

        // Act
        var result = await service.CreateDirectionAsync(new DictionaryModel { Name = "Программирование" });

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        var created = Assert.IsType<Direction>(ok.Value);
        Assert.Equal("Программирование", created.Name);

        // Направление действительно сохранено в БД
        var stored = Assert.Single(ctx.Directions);
        Assert.Equal("Программирование", stored.Name);
    }
}

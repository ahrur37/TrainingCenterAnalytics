using EduRequestSystemAPI.Models;
using EduRequestSystemAPI.Requests;
using EduRequestSystemAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace EduRequestSystemAPI.Tests;

public class DictionariesServiceTests
{
    [Fact]
    public async Task CreateDirectionAsync_WithEmptyName_ReturnsBadRequestAndSavesNothing()
    {
        await using var ctx = TestHelpers.CreateInMemoryContext();
        var service = new DictionariesService(ctx);

        var result = await service.CreateDirectionAsync(new DictionaryModel { Name = "   " });

        Assert.IsType<BadRequestObjectResult>(result);
        Assert.Empty(ctx.Directions);
    }

    [Fact]
    public async Task CreateDirectionAsync_WithValidName_PersistsDirection()
    {
        await using var ctx = TestHelpers.CreateInMemoryContext();
        var service = new DictionariesService(ctx);

        var result = await service.CreateDirectionAsync(new DictionaryModel { Name = "Программирование" });

        var ok = Assert.IsType<OkObjectResult>(result);
        var created = Assert.IsType<Direction>(ok.Value);
        Assert.Equal("Программирование", created.Name);

        var stored = Assert.Single(ctx.Directions);
        Assert.Equal("Программирование", stored.Name);
    }
}

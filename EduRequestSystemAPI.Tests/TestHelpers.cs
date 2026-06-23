using EduRequestSystemAPI.DatabaseContext;
using EduRequestSystemAPI.UniversalMethods;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EduRequestSystemAPI.Tests;

/// <summary>
/// Общие помощники для тестов: изолированный in-memory контекст БД
/// (без реального PostgreSQL) и настоящий генератор JWT с тестовым ключом.
/// </summary>
internal static class TestHelpers
{
    /// <summary>
    /// Создаёт свежий ContextDb на провайдере EF Core InMemory.
    /// Каждый вызов использует уникальное имя БД, поэтому тесты полностью изолированы.
    /// </summary>
    public static ContextDb CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<ContextDb>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ContextDb(options);
    }

    /// <summary>
    /// Создаёт реальный jwtGenerator с тестовым ключом (достаточной длины для HS256).
    /// </summary>
    public static jwtGenerator CreateJwtGenerator()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"] = "test_secret_key_for_jwt_at_least_32_bytes_long_1234567890"
            })
            .Build();

        return new jwtGenerator(configuration);
    }
}

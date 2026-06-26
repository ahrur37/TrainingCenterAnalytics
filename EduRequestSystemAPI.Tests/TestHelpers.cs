using EduRequestSystemAPI.DatabaseContext;
using EduRequestSystemAPI.UniversalMethods;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EduRequestSystemAPI.Tests;

internal static class TestHelpers
{
    public static ContextDb CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<ContextDb>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ContextDb(options);
    }

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

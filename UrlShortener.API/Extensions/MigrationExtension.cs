using Microsoft.EntityFrameworkCore;
using UrlShortener.API.Infrastructure;

namespace UrlShortener.API.Extensions;

public static class MigrationExtension
{
    public static void ApllyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        dbContext.Database.Migrate();
    }
}

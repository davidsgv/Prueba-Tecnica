using FlashLogistic.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace FlashLogistic.Infrastructure;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IHost app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        using ApplicationDbContext context =
            scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Database.Migrate();
    }
}
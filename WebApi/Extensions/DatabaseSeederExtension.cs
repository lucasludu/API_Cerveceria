using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Seed;
using System.Threading.Tasks;

namespace WebApi.Extensions
{
    public static class DatabaseSeederExtension
    {
        public static async Task SeedDatabaseAsync(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<Persistence.Contexts.ApplicationDbContext>();
                
                // As per requirements: EnsureDeleted and EnsureCreated to start fresh on every run
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                await IdentitySeed.SeedAsync(services);
            }
        }
    }
}

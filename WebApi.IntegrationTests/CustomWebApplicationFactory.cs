using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;
using System.Linq;

namespace WebApi.IntegrationTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptors = services.Where(
                    d => d.ServiceType.Name.Contains("DbContextOptions")).ToList();

                foreach (var d in descriptors)
                {
                    services.Remove(d);
                }

                var dbConnectionDescriptor = services.Where(
                    d => d.ServiceType.Name.Contains("DbConnection")).ToList();

                foreach (var d in dbConnectionDescriptor)
                {
                    services.Remove(d);
                }

                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });
            });
        }
    }
}

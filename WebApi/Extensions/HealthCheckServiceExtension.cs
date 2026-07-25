using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Text.Json;

namespace WebApi.Extensions
{
    public static class HealthCheckServiceExtension
    {
        public static void AddHealthCheckExtension(this IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddDbContextCheck<Persistence.Contexts.ApplicationDbContext>("SQL Server");
        }

        public static void UseHealthCheckExtension(this WebApplication app)
        {
            app.MapHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";
                    var result = JsonSerializer.Serialize(new
                    {
                        status = report.Status.ToString(),
                        checks = report.Entries.Select(e => new
                        {
                            name = e.Key,
                            status = e.Value.Status.ToString(),
                            description = e.Value.Description
                        })
                    });
                    await context.Response.WriteAsync(result);
                }
            });
        }
    }
}

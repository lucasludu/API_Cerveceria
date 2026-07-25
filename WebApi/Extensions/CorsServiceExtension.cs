using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Extensions
{
    public static class CorsServiceExtension
    {
        public static void AddCorsExtension(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });
        }
    }
}

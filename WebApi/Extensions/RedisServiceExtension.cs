using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Extensions
{
    public static class RedisServiceExtension
    {
        public static void AddRedisCacheExtension(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("RedisConnection") ?? "localhost:6379";
            });
        }
    }
}

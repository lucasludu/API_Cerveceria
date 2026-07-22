using Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Behaviours
{
    public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<CachingBehavior<TRequest, TResponse>> _logger;

        public CachingBehavior(IDistributedCache cache, ILogger<CachingBehavior<TRequest, TResponse>> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request is ICacheableQuery cacheableQuery)
            {
                var cacheKey = cacheableQuery.CacheKey;
                var cachedResponse = await _cache.GetStringAsync(cacheKey, cancellationToken);

                if (!string.IsNullOrEmpty(cachedResponse))
                {
                    _logger.LogInformation($"Fetched from Cache -> '{cacheKey}'.");
                    return JsonSerializer.Deserialize<TResponse>(cachedResponse)!;
                }

                _logger.LogInformation($"Cache miss -> '{cacheKey}'. Executing database query...");
                var response = await next();

                var options = new DistributedCacheEntryOptions();
                if (cacheableQuery.Expiration.HasValue)
                {
                    options.SetAbsoluteExpiration(cacheableQuery.Expiration.Value);
                }
                else
                {
                    // Default expiration if not specified
                    options.SetAbsoluteExpiration(System.TimeSpan.FromHours(1));
                }

                await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(response), options, cancellationToken);
                return response;
            }

            return await next();
        }
    }
}

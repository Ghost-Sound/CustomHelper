using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace CustomHelper.PipelineBehavior
{
    public class CacheBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IDistributedCache _cache;

        public CacheBehavior(IDistributedCache cache) => _cache = cache;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var key = $"{typeof(TRequest).FullName}:{GetRequestHashCode(request)}";

            var cachedResponse = await _cache.GetStringAsync(key);
            if (cachedResponse != null)
            {
                return JsonSerializer.Deserialize<TResponse>(cachedResponse);
            }

            var response = await next();
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                SlidingExpiration = TimeSpan.FromMinutes(1),
            };
            var json = JsonSerializer.Serialize(response);
            await _cache.SetStringAsync(key, json, options);

            return response;
        }

        private string GetRequestHashCode(TRequest request)
        {
            return JsonSerializer.Serialize(request).GetHashCode().ToString();
        }
    }
}

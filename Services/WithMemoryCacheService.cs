
namespace dotnet_HybridCache;
using Microsoft.Extensions.Caching.Memory;

public class WithMemoryCacheService : FakeBackEndDataService
{
    private readonly IMemoryCache _memoryCache;

    public WithMemoryCacheService(IMemoryCache memoryCache)
    {
        this._memoryCache = memoryCache;
    }

    public override async Task<FakeBackEndData> GetFakeExpensiveDataAsync(int id,
                                            CancellationToken cancellationToken)
    {
        var key = $"/somepath/{id}";
        if (!(_memoryCache.TryGetValue(key, out object? untyped) &&
                untyped is FakeBackEndData value))
        {
            // not available in cache, so add it
            value =  await GetFakeExpensiveDataAsync(id, cancellationToken);
            _memoryCache.Set(key, value, Expiration);
        }
        return value;
    }
}

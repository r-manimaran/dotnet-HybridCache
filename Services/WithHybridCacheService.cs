using Microsoft.Extensions.Caching.Hybrid;
using System.Threading;

namespace dotnet_HybridCache;

public class WithHybridCacheService : FakeBackEndDataService
{
    private readonly HybridCache _hybridCache;
    private const string KEY_PREFIX = "/somepath/";

    public WithHybridCacheService(HybridCache hybridCache)
    {
        _hybridCache = hybridCache ?? throw new ArgumentNullException(nameof(hybridCache));
    }

    public override async Task<FakeBackEndData> GetFakeExpensiveDataAsync(int id, 
        CancellationToken cancellationToken)
    {
        if (id < 0)
        {
            throw new ArgumentException("ID must be non-negative", nameof(id));
        }

        var key = $"{KEY_PREFIX}{id}";
        
        return await _hybridCache.GetOrCreateAsync(
            key: key,
            factory: async ct => await GetFakeExpensiveDataAsync(id, ct),
            cancellationToken: cancellationToken
        ).ConfigureAwait(false);
    }
}

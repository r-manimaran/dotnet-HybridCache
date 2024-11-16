using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace dotnet_HybridCache;

public class WithDistributedCacheService(IDistributedCache distributedCache):
                FakeBackEndDataService
{
    public override async Task<FakeBackEndData> GetFakeExpensiveDataAsync(int id, 
                                        CancellationToken cancellationToken)
    {
        var key= $"/somepath/{id}";
        var arr = await distributedCache.GetAsync(key, cancellationToken);
        
        FakeBackEndData? data = null;
        if (arr is not null)
        {
            return SomeSerializer.Deserialize<FakeBackEndData>(arr);
        }
        else{
            data = await base.GetFakeExpensiveDataAsync(id, cancellationToken);
            arr = SomeSerializer.Serialize(data);
            await distributedCache.SetAsync(key, arr, OptionsWithExpiration, cancellationToken);
        }
        return data;
    }

private static DistributedCacheEntryOptions OptionsWithExpiration =>
    new ()
    {
        AbsoluteExpirationRelativeToNow = Expiration
    };

static class SomeSerializer {
    public static byte[] Serialize<T>(T obj)
    {
        return JsonSerializer.SerializeToUtf8Bytes(obj);
    }
    public static T Deserialize<T>(byte[] arr)
    {
        return JsonSerializer.Deserialize<T>(arr)!;
    }
}
}

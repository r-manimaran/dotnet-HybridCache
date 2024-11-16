# dotnet-HybridCache
Different Cache Options (MemCache, Distributed Cache, new Hybrid Cache) in dotnet

Asp.net Core has traditionally offered two caching options.
1. In-Memory caching
2. Distributed caching

- In-Memory Cache with (IMemoryCache) is Fast, but limited to a single-server.
- Distributed caching  with (IDistributedCache) works across multiple servers.

.Net 9 introduces HybridCache, that combines the best of both approaches. It prevents the common caching problems like cache stampede.

**Hybrid-Cache**
 
**Important Feaures**
   - Two-level Caching (L1/L2)
     - L1: Fast In-Memory cache
     - L2: Distributed Cache (Redis)
   - Protection against cache stampede
   - Tag based cache invalidation
   - Configurable Serialization
   - Metrics and monitoring
  

L1 cache runs in application's memory.

L2 cache can be Redis,SQL Server etc.




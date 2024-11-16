namespace dotnet_HybridCache;

public class FakeBackEndDataService
{
  internal static TimeSpan Expiration = TimeSpan.FromSeconds(7.5);

  public virtual async Task<FakeBackEndData> GetFakeExpensiveDataAsync(int id, CancellationToken cancellationToken)
    => await DataFetchAsync(id, cancellationToken);


  protected async Task<FakeBackEndData>  DataFetchAsync(int id, CancellationToken cancellationToken)
  {
    //simulate a expensive fetch operation. In real code this may be a database fetch or complex calculations
    _ =id;

    await Task.Delay(TimeSpan.FromMilliseconds(2500), cancellationToken);

    // Create a Fake data response
    return new FakeBackEndData {
        Id = Guid.NewGuid(),
        CreationTime = DateTime.UtcNow,
        CacheProvider = GetType().Name,
    };
  }
}

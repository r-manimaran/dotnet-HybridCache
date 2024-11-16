using dotnet_HybridCache;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Comment the below line based on each caching Strategy
// 1. No cache, Just call the Service
builder.Services.AddSingleton<FakeBackEndDataService>();

// 2. InMemory Cache
builder.Services.AddMemoryCache();
//builder.Services.AddSingleton<FakeBackEndDataService, WithMemoryCacheService>();


// 3. Distributed Cache
   builder.Services.AddStackExchangeRedisCache(setup => {
    setup.Configuration =  "127.0.0.1:6379"; //builder.Configuration.GetConnectionString("Redis");
   });

   builder.Services.AddSingleton<FakeBackEndDataService, WithDistributedCacheService>();


// 4. Hybrid Cache
#pragma warning disable EXTEXP0018 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
builder.Services.AddHybridCache(options => {
        
        //set maximum size of the cache
        options.MaximumPayloadBytes = 1024 * 1024 * 10; //10MB
        options.MaximumKeyLength = 512;

        options.DefaultEntryOptions = new ()
        {
            LocalCacheExpiration = FakeBackEndDataService.Expiration,
            Expiration = FakeBackEndDataService.Expiration
        };
    });

// For Custom Type, we can add our own serializer
// builder.Services.AddHybridCacheSerializer<CustomType, CustomTypeSerializer>();
#pragma warning restore EXTEXP0018 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
//builder.Services.AddSingleton<FakeBackEndDataService, WithHybridCacheService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();

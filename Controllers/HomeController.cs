using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using dotnet_HybridCache.Models;
using dotnet_HybridCache;

namespace dotnet_HybridCache.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly FakeBackEndDataService _fakeBackEndDataService;

    public HomeController(ILogger<HomeController> logger,
                          FakeBackEndDataService fakeBackEndDataService)
    {
        _logger = logger;
        _fakeBackEndDataService = fakeBackEndDataService;
    }

    public async Task<IActionResult> IndexAsync(CancellationToken cancellationToken)
    {
        ViewData.Add("clock", DateTime.UtcNow);
        var timer = Stopwatch.StartNew();
        
        // get the fake data from the service
        var data = await _fakeBackEndDataService.GetFakeExpensiveDataAsync(40, cancellationToken);

        timer.Stop();
        ViewData.Add("timer", timer.ElapsedMilliseconds);
        return View(data);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

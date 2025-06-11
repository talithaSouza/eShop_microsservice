using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GeekShopping_Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using GeekShopping_Web.Services.IServices;

namespace GeekShopping_Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductService _productService;

    public HomeController(ILogger<HomeController> logger, IProductService productService)
    {
        _logger = logger;
        _productService = productService;
    }

    public async Task<IActionResult> IndexAsync()
    {
        var products = await _productService.GetAllProducts("");

        return View(products);
    }
   
   [Authorize]
    public async Task<IActionResult> Details(int id)
    {
        var token = await HttpContext.GetTokenAsync("access_token");

        var products = await _productService.GetProduct(id, token);

        return View(products);
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

    [Authorize]
    public async Task<IActionResult> Login()
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");

        System.Console.WriteLine($"Access: {accessToken}");
        return RedirectToAction(nameof(IndexAsync));
    }

    public IActionResult Logout()
    {
        return SignOut("Cookies", "oidc");
    }

}

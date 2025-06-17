using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GeekShopping_Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using GeekShopping_Web.Services.IServices;
using GeekShopping.Web.Services.IServices;

namespace GeekShopping_Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductService _productService;
    private readonly ICartService _cartService;

    public HomeController(ILogger<HomeController> logger, IProductService productService, ICartService cartService)
    {
        _logger = logger;
        _productService = productService;
        _cartService = cartService;
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

    [HttpPost]
    [ActionName("Details")]
    [Authorize]
    public async Task<IActionResult> DetailsPost(ProductViewModel model)
    {
        var token = await HttpContext.GetTokenAsync("access_token");

        CartViewModel cart = new()
        {
            CartHeader = new CartHeaderViewModel
            {
                UserId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value
            }
        };

        CartDetailViewModel cartDetail = new()
        {
            Count = model.Count,
            ProductId = model.Id,
            Product = await _productService.GetProduct(model.Id, token)
        };

        List<CartDetailViewModel> cartDetails = [cartDetail];
        
        cart.CartDetails = cartDetails;

        var response = await _cartService.AddItemToCart(cart, token);
        if (response != null)
        {
            return RedirectToAction(nameof(Index));
        }
        return View(model);
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

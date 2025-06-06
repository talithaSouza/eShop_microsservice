using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GeekShopping_Web.Models;
using GeekShopping_Web.Services.IServices;

namespace GeekShopping_Web.Controllers;

public class ProductController : Controller
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<IActionResult> ProductIndex()
    {
        var products = await _productService.GetAllProducts();
        return View(products);
    }
   

}

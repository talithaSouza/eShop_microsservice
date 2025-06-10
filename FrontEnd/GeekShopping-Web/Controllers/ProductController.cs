using Microsoft.AspNetCore.Mvc;
using GeekShopping_Web.Models;
using GeekShopping_Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using GeekShopping_Web.Utils;
using Microsoft.AspNetCore.Authentication;

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
        var products = await _productService.GetAllProducts("");
        return View(products);
    }

    public async Task<IActionResult> ProductCreate()
    {
        return View();
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> ProductCreate(ProductModel productModel)
    {
        if (ModelState.IsValid)
        {
            var token = await HttpContext.GetTokenAsync("access_token");

            var response = await _productService.CreateProduct(productModel, token);

            if (response != null)
                return RedirectToAction(nameof(ProductIndex));

        }

        return View(productModel);
    }
    public async Task<IActionResult> ProductUpdate(int id)
    {
        var token = await HttpContext.GetTokenAsync("access_token");

        var product = await _productService.GetProduct(id, token);
        if (product == null)
            return NotFound();

        return View(product);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> ProductUpdate(ProductModel productModel)
    {
        if (ModelState.IsValid)
        {
            var token = await HttpContext.GetTokenAsync("access_token");

            var response = await _productService.UpdateProduct(productModel, token);

            if (response != null)
                return RedirectToAction(nameof(ProductIndex));

        }

        return View(productModel);
    }

    [Authorize]
    public async Task<IActionResult> ProductDelete(int id)
    {
        var token = await HttpContext.GetTokenAsync("access_token");

        var product = await _productService.GetProduct(id,token);
        if (product == null)
            return NotFound();

        return View(product);
    }

    [HttpPost]
    [Authorize(Roles = Role.Admin)]
    public async Task<IActionResult> ProductDelete(ProductModel productModel)
    {
        var token = await HttpContext.GetTokenAsync("access_token");

        var response = await _productService.RemoveProductById(productModel.Id, token);

        if (response)
            return RedirectToAction(nameof(ProductIndex));

        return View(productModel);
    }
}

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

    public async Task<IActionResult> ProductCreate()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ProductCreate(ProductModel productModel)
    {
        if (ModelState.IsValid)
        {
            var response = await _productService.CreateProduct(productModel);

            if (response != null)
                return RedirectToAction(nameof(ProductIndex));

        }

        return View(productModel);
    }
    public async Task<IActionResult> ProductUpdate(int id)
    {
        var product = await _productService.GetProduct(id);
        if (product == null)
            return NotFound();

        return View(product);
    }

    [HttpPost]
    public async Task<IActionResult> ProductUpdate(ProductModel productModel)
    {
        if (ModelState.IsValid)
        {
            var response = await _productService.UpdateProduct(productModel);

            if (response != null)
                return RedirectToAction(nameof(ProductIndex));

        }

        return View(productModel);
    }
}

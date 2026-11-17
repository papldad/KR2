using BSUIR_Lychyts.Domain.Entities;
using BSUIR_Lychyts.Domain.Models;
using BSUIR_Lychyts.UI.Services.CategoryService;
using BSUIR_Lychyts.UI.Services.ProductService;
using Microsoft.AspNetCore.Mvc;

namespace BSUIR_Lychyts.UI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(string? category, int pageNo = 1)
        {
            var productResponse = await _productService.GetProductListAsync(category, pageNo);
            if (!productResponse.Successfull)
                return NotFound(productResponse.ErrorMessage);

            var categoryResponse = await _categoryService.GetCategoryListAsync();
            if (categoryResponse.Successfull)
            {
                ViewBag.Categories = categoryResponse.Data;
            }

            ViewBag.CurrentCategoryNormalized = category;

            if (!string.IsNullOrEmpty(category))
            {
                var currentCategory = categoryResponse.Data?
                    .FirstOrDefault(c => c.NormalizedName.Equals(category));
                ViewBag.CurrentCategory = currentCategory?.Name ?? "Все";
            }
            else
            {
                ViewBag.CurrentCategory = "Все";
            }

            return View(productResponse.Data);
        }
    }
}
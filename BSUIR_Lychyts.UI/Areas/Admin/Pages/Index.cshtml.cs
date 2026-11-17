using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BSUIR_Lychyts.Domain.Entities;
using BSUIR_Lychyts.Domain.Models;
using BSUIR_Lychyts.UI.Services.ProductService;

namespace BSUIR_Lychyts.UI.Areas.Admin.Pages.Dishes
{
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService;

        public IndexModel(IProductService productService)
        {
            _productService = productService;
        }

        public ListModel<Dish> Dishes { get; set; } = new();

        public async Task OnGetAsync()
        {
            
            var response = await _productService.GetProductListAsync(null, 1, 999);
            if (response.Successfull && response.Data != null)
            {
                Dishes = response.Data;
            }
        }
    }
}
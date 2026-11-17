using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BSUIR_Lychyts.Domain.Entities;
using BSUIR_Lychyts.UI.Services.ProductService;

namespace BSUIR_Lychyts.UI.Areas.Admin.Pages.Dishes
{
    public class DeleteModel : PageModel
    {
        private readonly IProductService _productService;

        public DeleteModel(IProductService productService)
        {
            _productService = productService;
        }

        [BindProperty]
        public Dish Dish { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            var response = await _productService.GetProductByIdAsync(id.Value);
            if (response.Data == null) return NotFound();

            Dish = response.Data;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null) return NotFound();

            await _productService.DeleteProductAsync(id.Value);
            return RedirectToPage("./Index");
        }
    }
}
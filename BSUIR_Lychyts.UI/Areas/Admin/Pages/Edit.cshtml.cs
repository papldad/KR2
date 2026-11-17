using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BSUIR_Lychyts.Domain.Entities;
using BSUIR_Lychyts.UI.Services.ProductService;

namespace BSUIR_Lychyts.UI.Areas.Admin.Pages.Dishes
{
    public class EditModel : PageModel
    {
        private readonly IProductService _productService;

        public EditModel(IProductService productService)
        {
            _productService = productService;
        }

        [BindProperty]
        public Dish Dish { get; set; } = new();

        [BindProperty]
        public IFormFile? Image { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            var response = await _productService.GetProductByIdAsync(id.Value);
            if (response.Data == null) return NotFound();

            Dish = response.Data;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid) return Page();

            await _productService.UpdateProductAsync(id, Dish, Image);
            return RedirectToPage("./Index");
        }
    }
}
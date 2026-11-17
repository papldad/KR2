using BSUIR_Lychyts.API.Services;
using BSUIR_Lychyts.Domain.Entities;
using BSUIR_Lychyts.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace BSUIR_Lychyts.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishesController : ControllerBase
    {
        private readonly IProductService _productService;

        public DishesController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/Dishes
        [HttpGet]
        public async Task<ActionResult<ResponseData<ListModel<Dish>>>> GetDishes(string? category, int pageNo = 1, int pageSize = 3)
        {
            var result = await _productService.GetProductListAsync(category, pageNo, pageSize);
            return Ok(result);
        }

        // GET: api/Dishes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseData<Dish>>> GetDish(int id)
        {
            var result = await _productService.GetProductByIdAsync(id);
            if (!result.Successfull) return NotFound(result);
            return Ok(result);
        }

        // PUT: api/Dishes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDish(int id, Dish dish)
        {
            await _productService.UpdateProductAsync(id, dish);
            return NoContent();
        }

        // POST: api/Dishes
        [HttpPost]
        public async Task<ActionResult<ResponseData<Dish>>> PostDish(Dish dish)
        {
            var result = await _productService.CreateProductAsync(dish);
            return CreatedAtAction(nameof(GetDish), new { id = result.Data.Id }, result);
        }

        // DELETE: api/Dishes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDish(int id)
        {
            await _productService.DeleteProductAsync(id);
            return NoContent();
        }
    }
}
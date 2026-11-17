using BSUIR_Lychyts.API.Services;
using BSUIR_Lychyts.Domain.Entities;
using BSUIR_Lychyts.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace BSUIR_Lychyts.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseData<List<Category>>>> GetCategories()
        {
            var result = await _categoryService.GetCategoryListAsync();
            return Ok(result);
        }
    }
}
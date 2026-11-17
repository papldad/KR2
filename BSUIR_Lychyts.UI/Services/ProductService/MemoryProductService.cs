using BSUIR_Lychyts.Domain.Entities;
using BSUIR_Lychyts.Domain.Models;
using BSUIR_Lychyts.UI.Services.CategoryService;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace BSUIR_Lychyts.UI.Services.ProductService
{
    public class MemoryProductService : IProductService
    {
        List<Dish> _dishes = new();
        List<Category> _categories = new();
        private readonly int _pageSize;

        public MemoryProductService(
            IConfiguration config,
            ICategoryService categoryService)
        {
            _categories = categoryService.GetCategoryListAsync().Result.Data!;
            _pageSize = int.Parse(config["ItemsPerPage"] ?? "3");
            SetupData();
        }

        private void SetupData()
        {
            _dishes = new List<Dish>
            {
                new Dish
                {
                    Id = 1,
                    Name = "Суп-харчо",
                    Description = "Очень острый, невкусный",
                    Calories = 200,
                    Image = "Images/soup.jpg",
                    Category = _categories.Find(c => c.NormalizedName.Equals("soups"))
                },
                new Dish
                {
                    Id = 2,
                    Name = "Борщ",
                    Description = "Много сала, без сметаны",
                    Calories = 330,
                    Image = "Images/borsch.jpg",
                    Category = _categories.Find(c => c.NormalizedName.Equals("soups"))
                },
                new Dish
                {
                    Id = 3,
                    Name = "Цезарь",
                    Description = "Салат с курицей",
                    Calories = 180,
                    Image = "Images/caesar.jpg",
                    Category = _categories.Find(c => c.NormalizedName.Equals("salads"))
                },
                new Dish
                {
                    Id = 4,
                    Name = "Кола",
                    Description = "Холодный напиток",
                    Calories = 42,
                    Image = "Images/cola.jpg",
                    Category = _categories.Find(c => c.NormalizedName.Equals("drinks"))
                },
                new Dish
                {
                    Id = 5,
                    Name = "Стейк",
                    Description = "Говяжий стейк medium rare",
                    Calories = 250,
                    Image = "Images/steak.jpg",
                    Category = _categories.Find(c => c.NormalizedName.Equals("main-dishes"))
                },
                new Dish
                {
                    Id = 6,
                    Name = "Тирамису",
                    Description = "Итальянский десерт",
                    Calories = 450,
                    Image = "Images/tiramisu.jpg",
                    Category = _categories.Find(c => c.NormalizedName.Equals("desserts"))
                }
            };
        }

        public Task<ResponseData<ListModel<Dish>>> GetProductListAsync(
    string? categoryNormalizedName, int pageNo = 1, int pageSize = 3)
        {
            var data = _dishes
                .Where(d => categoryNormalizedName == null ||
                            d.Category!.NormalizedName.Equals(categoryNormalizedName))
                .ToList();

            int totalPages = (int)Math.Ceiling(data.Count / (double)pageSize);
            if (totalPages == 0) totalPages = 1;

            if (pageNo < 1) pageNo = 1;
            if (pageNo > totalPages) pageNo = totalPages;

            var pagedData = data
    .Skip((pageNo - 1) * pageSize)
    .Take(pageSize)
    .ToList();

            var result = new ListModel<Dish>
            {
                Items = pagedData,
                CurrentPage = pageNo,
                TotalPages = totalPages
            };

            return Task.FromResult(ResponseData<ListModel<Dish>>.Success(result));
        }

        public Task<ResponseData<Dish>> GetProductByIdAsync(int id)
        {
            var dish = _dishes.Find(d => d.Id == id);
            if (dish == null)
                return Task.FromResult(ResponseData<Dish>.Error("Не найдено"));

            return Task.FromResult(ResponseData<Dish>.Success(dish));
        }

        public Task UpdateProductAsync(int id, Dish product, IFormFile? formFile)
        {
            var dish = _dishes.Find(d => d.Id == id);
            if (dish != null)
            {
                dish.Name = product.Name;
                dish.Description = product.Description;
                dish.Calories = product.Calories;
            }
            return Task.CompletedTask;
        }

        public Task DeleteProductAsync(int id)
        {
            var dish = _dishes.Find(d => d.Id == id);
            if (dish != null)
                _dishes.Remove(dish);
            return Task.CompletedTask;
        }

        public Task<ResponseData<Dish>> CreateProductAsync(Dish product, IFormFile? formFile)
        {
            product.Id = _dishes.Max(d => d.Id) + 1;
            _dishes.Add(product);
            return Task.FromResult(ResponseData<Dish>.Success(product));
        }
    }
}
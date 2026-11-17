using BSUIR_Lychyts.API.Data;
using BSUIR_Lychyts.Domain.Entities;
using BSUIR_Lychyts.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace BSUIR_Lychyts.API.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly int _maxPageSize = 20;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseData<ListModel<Dish>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3)
        {
            if (pageSize > _maxPageSize) pageSize = _maxPageSize;

            var query = _context.Dishes.Include(d => d.Category).AsQueryable();

            if (!string.IsNullOrEmpty(categoryNormalizedName))
            {
                query = query.Where(d => d.Category != null && d.Category.NormalizedName.Equals(categoryNormalizedName));
            }

            var count = await query.CountAsync();
            var dataList = new ListModel<Dish>();

            if (count == 0)
            {
                return ResponseData<ListModel<Dish>>.Success(dataList);
            }

            int totalPages = (int)Math.Ceiling(count / (double)pageSize);
            if (pageNo > totalPages)
            {
                return ResponseData<ListModel<Dish>>.Error("Нет такой страницы");
            }

            dataList.Items = await query
                .OrderBy(d => d.Id)
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            dataList.CurrentPage = pageNo;
            dataList.TotalPages = totalPages;

            return ResponseData<ListModel<Dish>>.Success(dataList);
        }

        public async Task<ResponseData<Dish>> GetProductByIdAsync(int id)
        {
            var dish = await _context.Dishes.Include(d => d.Category).FirstOrDefaultAsync(d => d.Id == id);
            if (dish == null) return ResponseData<Dish>.Error("Не найдено");
            return ResponseData<Dish>.Success(dish);
        }

        public async Task UpdateProductAsync(int id, Dish product)
        {
            var dish = await _context.Dishes.FindAsync(id);
            if (dish != null)
            {
                dish.Name = product.Name;
                dish.Description = product.Description;
                dish.Calories = product.Calories;
                dish.CategoryId = product.CategoryId;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            var dish = await _context.Dishes.FindAsync(id);
            if (dish != null)
            {
                _context.Dishes.Remove(dish);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<ResponseData<Dish>> CreateProductAsync(Dish product)
        {
            _context.Dishes.Add(product);
            await _context.SaveChangesAsync();
            return ResponseData<Dish>.Success(product);
        }

        public Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
        {
            
            return Task.FromResult(ResponseData<string>.Success(""));
        }
    }
}
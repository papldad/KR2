using BSUIR_Lychyts.Domain.Entities;
using BSUIR_Lychyts.Domain.Models;
using BSUIR_Lychyts.UI.Services.FileService;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

namespace BSUIR_Lychyts.UI.Services.ProductService
{
    public class ApiProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly IFileService _fileService;
        private readonly string _pageSize;
        private readonly JsonSerializerOptions _jsonOptions;

        public ApiProductService(HttpClient httpClient, IConfiguration configuration, IFileService fileService)
        {
            _httpClient = httpClient;
            _fileService = fileService;
            _pageSize = configuration["ItemsPerPage"] ?? "3";
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<ResponseData<ListModel<Dish>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3)
        {
            var url = new StringBuilder($"Dishes?pageNo={pageNo}&pageSize={pageSize}");
            if (!string.IsNullOrEmpty(categoryNormalizedName))
            {
                url.Append($"&category={categoryNormalizedName}");
            }

            var response = await _httpClient.GetAsync(url.ToString());
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Dish>>>(_jsonOptions);
                return result ?? ResponseData<ListModel<Dish>>.Error("Пустой ответ от сервера");
            }
            return ResponseData<ListModel<Dish>>.Error($"Ошибка HTTP: {response.StatusCode}");
        }
        public async Task<ResponseData<Dish>> GetProductByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"Dishes/{id}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ResponseData<Dish>>(_jsonOptions);
                return result ?? ResponseData<Dish>.Error("Пустой ответ от сервера");
            }
            return ResponseData<Dish>.Error($"Ошибка HTTP: {response.StatusCode}");
        }

        public async Task UpdateProductAsync(int id, Dish product, IFormFile? formFile)
        {
            if (formFile != null)
            {
                if (!string.IsNullOrEmpty(product.Image))
                {
                    await _fileService.DeleteFileAsync(product.Image);
                }
                var imageUrl = await _fileService.SaveFileAsync(formFile);
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    product.Image = imageUrl;
                }
            }

            var response = await _httpClient.PutAsJsonAsync($"Dishes/{id}", product, _jsonOptions);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteProductAsync(int id)
        {
            var dishResponse = await GetProductByIdAsync(id);
            if (dishResponse.Successfull && dishResponse.Data != null && !string.IsNullOrEmpty(dishResponse.Data.Image))
            {
                await _fileService.DeleteFileAsync(dishResponse.Data.Image);
            }

            var response = await _httpClient.DeleteAsync($"Dishes/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<ResponseData<Dish>> CreateProductAsync(Dish product, IFormFile? formFile)
        {
            product.Image = "https://localhost:7002/Images/noimage.jpg";

            if (formFile != null)
            {
                var imageUrl = await _fileService.SaveFileAsync(formFile);
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    product.Image = imageUrl;
                }
            }

            var response = await _httpClient.PostAsJsonAsync("Dishes", product, _jsonOptions);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ResponseData<Dish>>(_jsonOptions);
                return result ?? ResponseData<Dish>.Error("Пустой ответ от сервера");
            }
            return ResponseData<Dish>.Error($"Ошибка HTTP: {response.StatusCode}");
        }
    }
}
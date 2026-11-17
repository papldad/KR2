using BSUIR_Lychyts.Domain.Entities;
using BSUIR_Lychyts.Domain.Models;
using System.Text.Json;

namespace BSUIR_Lychyts.UI.Services.CategoryService
{
    public class ApiCategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public ApiCategoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            var response = await _httpClient.GetAsync("Categories");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ResponseData<List<Category>>>(_jsonOptions);
                return result ?? ResponseData<List<Category>>.Error("Пустой ответ от сервера");
            }
            return ResponseData<List<Category>>.Error($"Ошибка HTTP: {response.StatusCode}");
        }
    }
}
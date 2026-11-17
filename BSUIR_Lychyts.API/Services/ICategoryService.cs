using BSUIR_Lychyts.Domain.Entities;
using BSUIR_Lychyts.Domain.Models;

namespace BSUIR_Lychyts.API.Services
{
    public interface ICategoryService
    {
        Task<ResponseData<List<Category>>> GetCategoryListAsync();
    }
}
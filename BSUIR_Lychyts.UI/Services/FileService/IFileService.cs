using Microsoft.AspNetCore.Http;

namespace BSUIR_Lychyts.UI.Services.FileService
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile formFile);
        Task DeleteFileAsync(string fileUri);
    }
}
using BSUIR_Lychyts.UI.Models;
using BSUIR_Lychyts.UI.Services.CategoryService;
using BSUIR_Lychyts.UI.Services.ProductService;
using BSUIR_Lychyts.UI.Services.FileService;  
using Microsoft.EntityFrameworkCore;

namespace BSUIR_Lychyts.UI.Extensions
{
    public static class HostingExtensions
    {
        public static void RegisterCustomServices(this WebApplicationBuilder builder)
        {
            var uriData = builder.Configuration.GetSection("UriData").Get<UriData>() ?? new UriData();
            var apiUri = new Uri(uriData.ApiUri);

            builder.Services.AddHttpClient<ICategoryService, ApiCategoryService>(opt =>
            {
                opt.BaseAddress = apiUri;
            });

            builder.Services.AddHttpClient<IProductService, ApiProductService>(opt =>
            {
                opt.BaseAddress = apiUri;
            });

            builder.Services.AddHttpClient<IFileService, ApiFileService>(opt =>
            {
                opt.BaseAddress = new Uri($"{uriData.ApiUri}Files/");
            });
        }
    }
}
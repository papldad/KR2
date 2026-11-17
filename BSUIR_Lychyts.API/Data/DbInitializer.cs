using BSUIR_Lychyts.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BSUIR_Lychyts.API.Data
{
    public static class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await context.Database.MigrateAsync();

            var apiUrl = app.Configuration["ApiUrl"];

            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Закуски", NormalizedName = "zakuski" },
                    new Category { Name = "Салаты", NormalizedName = "salads" },
                    new Category { Name = "Супы", NormalizedName = "soups" },
                    new Category { Name = "Основные блюда", NormalizedName = "main-dishes" },
                    new Category { Name = "Напитки", NormalizedName = "drinks" },
                    new Category { Name = "Десерты", NormalizedName = "desserts" }
                };
                context.Categories.AddRange(categories);
                await context.SaveChangesAsync();
            }

            if (!context.Dishes.Any())
            {
                var soups = await context.Categories.FirstOrDefaultAsync(c => c.NormalizedName == "soups");
                var salads = await context.Categories.FirstOrDefaultAsync(c => c.NormalizedName == "salads");
                var drinks = await context.Categories.FirstOrDefaultAsync(c => c.NormalizedName == "drinks");
                var mainDishes = await context.Categories.FirstOrDefaultAsync(c => c.NormalizedName == "main-dishes");
                var desserts = await context.Categories.FirstOrDefaultAsync(c => c.NormalizedName == "desserts");

                var dishes = new List<Dish>
                {
                    new Dish { Name = "Суп-харчо", Description = "Очень острый", Calories = 200, Image = $"{apiUrl}/Images/soup.jpg", Category = soups },
                    new Dish { Name = "Борщ", Description = "Много сала, без сметаны", Calories = 330, Image = $"{apiUrl}/Images/borsch.jpg", Category = soups },
                    new Dish { Name = "Цезарь", Description = "Салат с курицей", Calories = 180, Image = $"{apiUrl}/Images/caesar.jpg", Category = salads },
                    new Dish { Name = "Кола", Description = "Холодный напиток", Calories = 42, Image = $"{apiUrl}/Images/cola.jpg", Category = drinks },
                    new Dish { Name = "Стейк", Description = "Говяжий стейк medium rare", Calories = 250, Image = $"{apiUrl}/Images/steak.jpg", Category = mainDishes },
                    new Dish { Name = "Тирамису", Description = "Итальянский десерт", Calories = 450, Image = $"{apiUrl}/Images/tiramisu.jpg", Category = desserts }
                };
                context.Dishes.AddRange(dishes);
                await context.SaveChangesAsync();
            }
        }
    }
}
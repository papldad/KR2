using Microsoft.EntityFrameworkCore;
using BSUIR_Lychyts.Domain.Entities;

namespace BSUIR_Lychyts.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Dish> Dishes { get; set; }
    }
}
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Product.API.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Models.Product> Products { get; set; }
}
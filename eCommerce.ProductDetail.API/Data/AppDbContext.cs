using Microsoft.EntityFrameworkCore;

namespace eCommerce.ProductDetail.API.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Models.ProductDetail> ProductDetails { get; set; }
}
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Inventory.API.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Models.Inventory> Inventories { get; set; }
}
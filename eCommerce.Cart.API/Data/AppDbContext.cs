using eCommerce.Cart.API.Models;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Cart.API.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
}
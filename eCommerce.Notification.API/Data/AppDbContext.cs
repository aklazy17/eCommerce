using Microsoft.EntityFrameworkCore;

namespace eCommerce.Notification.API.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Models.Notification> Notifications { get; set; }
}
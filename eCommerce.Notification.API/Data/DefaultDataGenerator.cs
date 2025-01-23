using eCommerce.Notification.API.Enums;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Notification.API.Data;

public static class DefaultDataGenerator
{
    public static IHost MigrateDatabase(this IHost webHost)
    {
        using (var scope = webHost.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<AppDbContext>();

            // Apply pending migrations
            context.Database.Migrate();

            SeedDefaultData(context);
        }

        return webHost;
    }

    public static void SeedDefaultData(AppDbContext context)
    {
        // Check if there is any notifications
        if (context.Notifications.Any())
        {
            return; // Default data was already seeded
        }

        context.Notifications.AddRange(
            new Models.Notification
            {
                Id = Guid.NewGuid(),
                EventType = EventType.LowStock,
                Message = "Polo T-Shirt stock is running low.",
                Recipient = "admin@ecommerce.com",
                CreatedOn = DateTime.Now
            },
            new Models.Notification
            {
                Id = Guid.NewGuid(),
                EventType = EventType.ProductAdded,
                Message = "A new product has been added to the stock: Denim Shirt.",
                Recipient = "admin@ecommerce.com",
                CreatedOn = DateTime.Now
            },
            new Models.Notification
            {
                Id = Guid.NewGuid(),
                EventType = EventType.ProductAdded,
                Message = "A new product has been added to the stock: Gabru Punjabi Kurta.",
                Recipient = "admin@ecommerce.com",
                CreatedOn = DateTime.Now
            },
            new Models.Notification
            {
                Id = Guid.NewGuid(),
                EventType = EventType.CheckoutSuccess,
                Message = "The order has been successfully checked out.",
                Recipient = "xyz@testmail.com",
                CreatedOn = DateTime.Now
            });

        context.SaveChanges();
    }
}
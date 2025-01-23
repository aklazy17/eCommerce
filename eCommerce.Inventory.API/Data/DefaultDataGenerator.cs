using Microsoft.EntityFrameworkCore;

namespace eCommerce.Inventory.API.Data;

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
        // Check if there is any inventories
        if (context.Inventories.Any())
        {
            return; // Default data was already seeded
        }

        context.Inventories.AddRange(
            new Models.Inventory
            {
                Id = Guid.NewGuid(),
                ProductId = new Guid("cf6ae843-5c8a-4067-9919-1873730af45c"),
                Quantity = 10,
                CreatedOn = DateTime.Now,
                LastUpdatedOn = DateTime.Now,
            },
            new Models.Inventory
            {
                Id = Guid.NewGuid(),
                ProductId = new Guid("15c2ac05-a9e1-4e5a-ad3e-04544874333f"),
                Quantity = 7,
                CreatedOn = DateTime.Now,
                LastUpdatedOn = DateTime.Now,
            },
            new Models.Inventory
            {
                Id = Guid.NewGuid(),
                ProductId = new Guid("7a4ce26d-6947-48ad-9706-d76efffe4e7d"),
                Quantity = 20,
                CreatedOn = DateTime.Now,
                LastUpdatedOn = DateTime.Now,
            },
            new Models.Inventory
            {
                Id = Guid.NewGuid(),
                ProductId = new Guid("589a1eec-2f50-420a-9fd1-46441e434cce"),
                Quantity = 14,
                CreatedOn = DateTime.Now,
                LastUpdatedOn = DateTime.Now,
            },
            new Models.Inventory
            {
                Id = Guid.NewGuid(),
                ProductId = new Guid("ca2cd151-17c0-4988-a960-77eb71bd02f1"),
                Quantity = 0,
                CreatedOn = DateTime.Now,
                LastUpdatedOn = DateTime.Now,
            });

        context.SaveChanges();
    }
}
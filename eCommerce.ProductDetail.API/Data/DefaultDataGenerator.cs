using Microsoft.EntityFrameworkCore;

namespace eCommerce.ProductDetail.API.Data;

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
        // Check if there is any product details
        if (context.ProductDetails.Any())
        {
            return; // Default data was already seeded
        }

        context.ProductDetails.AddRange(
            new Models.ProductDetail
            {
                Id = Guid.NewGuid(),
                ProductId = new Guid("cf6ae843-5c8a-4067-9919-1873730af45c"),
                Price = 499.99,
                Size = "Small",
                Design = "Regular Fit",
                CreatedOn = DateTime.Now,
                LastUpdatedOn = DateTime.Now
            },
            new Models.ProductDetail
            {
                Id = Guid.NewGuid(),
                ProductId = new Guid("cf6ae843-5c8a-4067-9919-1873730af45c"),
                Price = 499.99,
                Size = "Medium",
                Design = "Regular Fit",
                CreatedOn = DateTime.Now,
                LastUpdatedOn = DateTime.Now
            },
            new Models.ProductDetail
            {
                Id = Guid.NewGuid(),
                ProductId = new Guid("cf6ae843-5c8a-4067-9919-1873730af45c"),
                Price = 479.99,
                Size = "Large",
                Design = "Regular Fit",
                CreatedOn = DateTime.Now,
                LastUpdatedOn = DateTime.Now
            },
            new Models.ProductDetail
            {
                Id = Guid.NewGuid(),
                ProductId = new Guid("15c2ac05-a9e1-4e5a-ad3e-04544874333f"),
                Price = 799.49,
                Size = "28",
                Design = "Slim Fit",
                CreatedOn = DateTime.Now,
                LastUpdatedOn = DateTime.Now
            },
            new Models.ProductDetail
            {
                Id = Guid.NewGuid(),
                ProductId = new Guid("15c2ac05-a9e1-4e5a-ad3e-04544874333f"),
                Price = 799.49,
                Size = "30",
                Design = "Slim Fit",
                CreatedOn = DateTime.Now,
                LastUpdatedOn = DateTime.Now
            },
            new Models.ProductDetail
            {
                Id = Guid.NewGuid(),
                ProductId = new Guid("15c2ac05-a9e1-4e5a-ad3e-04544874333f"),
                Price = 799.49,
                Size = "32",
                Design = "Slim Fit",
                CreatedOn = DateTime.Now,
                LastUpdatedOn = DateTime.Now
            },
            new Models.ProductDetail
            {
                Id = Guid.NewGuid(),
                ProductId = new Guid("7a4ce26d-6947-48ad-9706-d76efffe4e7d"),
                Price = 299.00,
                Size = "Small",
                Design = "Slim Fit",
                CreatedOn = DateTime.Now,
                LastUpdatedOn = DateTime.Now
            },
            new Models.ProductDetail
            {
                Id = Guid.NewGuid(),
                ProductId = new Guid("7a4ce26d-6947-48ad-9706-d76efffe4e7d"),
                Price = 299.00,
                Size = "Medium",
                Design = "Slim Fit",
                CreatedOn = DateTime.Now,
                LastUpdatedOn = DateTime.Now
            },
            new Models.ProductDetail
            {
                Id = Guid.NewGuid(),
                ProductId = new Guid("7a4ce26d-6947-48ad-9706-d76efffe4e7d"),
                Price = 299.00,
                Size = "Large",
                Design = "Slim Fit",
                CreatedOn = DateTime.Now,
                LastUpdatedOn = DateTime.Now
            },
            new Models.ProductDetail
            {
                Id = Guid.NewGuid(),
                ProductId = new Guid("589a1eec-2f50-420a-9fd1-46441e434cce"),
                Price = 1249.00,
                Size = "38",
                Design = "Party Wear",
                CreatedOn = DateTime.Now,
                LastUpdatedOn = DateTime.Now
            },
            new Models.ProductDetail
            {
                Id = Guid.NewGuid(),
                ProductId = new Guid("589a1eec-2f50-420a-9fd1-46441e434cce"),
                Price = 1249.00,
                Size = "40",
                Design = "Party Wear",
                CreatedOn = DateTime.Now,
                LastUpdatedOn = DateTime.Now
            },
            new Models.ProductDetail
            {
                Id = Guid.NewGuid(),
                ProductId = new Guid("ca2cd151-17c0-4988-a960-77eb71bd02f1"),
                Price = 732.99,
                Size = "Free Size",
                Design = "Punjab Style Kurta",
                CreatedOn = DateTime.Now,
                LastUpdatedOn = DateTime.Now
            });

        context.SaveChanges();
    }
}
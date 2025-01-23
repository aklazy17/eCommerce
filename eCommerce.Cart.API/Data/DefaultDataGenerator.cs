using eCommerce.Cart.API.Models;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Cart.API.Data;

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
        // Check if there is any orders
        if (context.Orders.Any())
        {
            return; // Default data was already seeded
        }

        List<Order> tempOrders =
            [
                new Order
                    {
                        CreatedDate = DateTime.Now,
                        Total = 0,
                        CheckoutCompleted = true,
                        Details =
                        [
                            new() {
                                ProductId = new Guid("cf6ae843-5c8a-4067-9919-1873730af45c"),
                                Price = 499.99,
                                Quantity = 2
                            },
                            new() {
                                ProductId = new Guid("15c2ac05-a9e1-4e5a-ad3e-04544874333f"),
                                Price = 799.49,
                                Quantity = 1
                            }
                        ]
                    },
                    new Order
                    {
                        CreatedDate = DateTime.Now,
                        Total = 0,
                        CheckoutCompleted = true,
                        Details =
                        [
                            new() {
                                ProductId = new Guid("7a4ce26d-6947-48ad-9706-d76efffe4e7d"),
                                Price = 499.99,
                                Quantity = 3
                            },
                            new() {
                                ProductId = new Guid("15c2ac05-a9e1-4e5a-ad3e-04544874333f"),
                                Price = 799.49,
                                Quantity = 2
                            },
                            new() {
                                ProductId = new Guid("589a1eec-2f50-420a-9fd1-46441e434cce"),
                                Price = 1249.00,
                                Quantity = 1
                            }
                        ]
                    }
            ];

        tempOrders.ForEach(tempOrder => tempOrder.Total = tempOrder.Details.Sum(d => d.Price * d.Quantity));

        context.Orders.AddRange(tempOrders);
        context.SaveChanges();
    }
}
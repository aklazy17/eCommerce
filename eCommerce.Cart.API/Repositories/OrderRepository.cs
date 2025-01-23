using eCommerce.Cart.API.Data;
using eCommerce.Cart.API.Models;
using eCommerce.Cart.API.Services;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Cart.API.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _db;
    private readonly ILogger<OrderRepository> _logger;
    private readonly IInventoryService _inventoryService;

    public OrderRepository(AppDbContext db,
        ILogger<OrderRepository> logger,
        IInventoryService inventoryService)
    {
        _db = db;
        _logger = logger;
        _inventoryService = inventoryService;
    }

    public async Task<Order> AddToCartAsync(Guid? orderId, OrderDetail orderDetail)
    {
        var order = await GetOrderAsync(orderId ?? Guid.Empty);
        if (order is null)
        {
            // Create order
            order = new Order
            {
                CreatedDate = DateTime.Now,
                Details = [orderDetail],
                Total = orderDetail.Quantity * orderDetail.Price
            };

            _db.Orders.Add(order);
        }
        else
        {
            if (order.CheckoutCompleted)
            {
                _logger?.LogInformation($"Order Id: {orderId}");
                _logger?.LogInformation("The item cannot be added because the order has already been completed.");
                throw new InvalidOperationException("The item cannot be added because the order has already been completed.");
            }

            if (order.Details.Any(x => x.ProductId == orderDetail.ProductId))
            {
                var oDetail = order.Details.First(x => x.ProductId == orderDetail.ProductId);
                oDetail.Quantity = orderDetail.Quantity;
                oDetail.Price = orderDetail.Price;
                _db.OrderDetails.Update(oDetail);
            }
            else
            {
                // Add new item to cart
                orderDetail.OrderId = order.Id;
                _db.OrderDetails.Add(orderDetail);
            }

            order.Total = order.Details.Sum(d => d.Price * d.Quantity);
            _db.Orders.Update(order);
        }

        await _db.SaveChangesAsync();

        return order;
    }

    public async Task RemoveCartAsync(Guid orderId)
    {
        var order = await GetOrderAsync(orderId) ?? throw new KeyNotFoundException("Cart not found.");
        if (order is not null)
        {
            if (order.CheckoutCompleted)
            {
                _logger?.LogInformation($"Order Id: {orderId}");
                _logger?.LogInformation("The cart cannot be removed because the order has already been completed.");
                throw new InvalidOperationException("The cart cannot be removed because the order has already been completed.");
            }

            _db.Orders.Remove(order);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<Order?> RemoveCartItemAsync(Guid orderId, Guid orderDetailId)
    {
        var order = await GetOrderAsync(orderId) ?? throw new KeyNotFoundException("Cart not found.");
        if (order is not null)
        {
            if (order.CheckoutCompleted)
            {
                _logger?.LogInformation($"Order Id: {orderId}");
                _logger?.LogInformation("The item cannot be removed because the order has already been completed.");
                throw new InvalidOperationException("The item cannot be removed because the order has already been completed.");
            }

            var item = order.Details.FirstOrDefault(x => x.Id == orderDetailId) ?? throw new KeyNotFoundException("Item not found");

            _db.OrderDetails.Remove(item);

            order.Total = order.Details.Where(x => x.Id != orderDetailId).Sum(d => d.Price * d.Quantity);
            _db.Orders.Update(order);

            await _db.SaveChangesAsync();
        }

        return order;
    }

    public async Task<Order?> CheckoutAsync(Guid orderId)
    {
        var order = await GetOrderAsync(orderId) ?? throw new KeyNotFoundException("Cart not found.");
        if (order is not null)
        {
            if (order.Details.Count == 0)
            {
                _logger?.LogInformation("There are no items in your cart.");
                throw new InvalidOperationException("There are no items in your cart.");
            }

            if (order.CheckoutCompleted)
            {
                _logger?.LogInformation($"Order Id: {orderId}");
                _logger?.LogInformation("Order is already been completed.");
                throw new InvalidOperationException("Order is already been completed.");
            }

            #region Update Quantity in Inventory

            try
            {
                order.Details.ForEach(x =>
                {
                    _inventoryService.UpdateQuantityByProductIdAsync(x.ProductId, x.Quantity);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failure occured while updating inventory.");
                _logger.LogError($"{ex}");
                throw new Exception(ex.Message);
            }

            #endregion

            order.CheckoutCompleted = true;

            _db.Orders.Update(order);
            await _db.SaveChangesAsync();
        }

        return order;
    }

    public async Task<Order?> GetOrderAsync(Guid orderId)
    {
        return await _db.Orders.Include(x => x.Details).FirstOrDefaultAsync(x => x.Id == orderId);
    }

    public async Task<List<Order>> GetAsync()
    {
        return await _db.Orders.Include(x => x.Details).ToListAsync();
    }
}
using eCommerce.Cart.API.Models;

namespace eCommerce.Cart.API.Repositories;

public interface IOrderRepository
{
    Task<Order> AddToCartAsync(Guid? orderId, OrderDetail orderDetail);
    Task RemoveCartAsync(Guid orderId);
    Task<Order?> RemoveCartItemAsync(Guid orderId, Guid orderDetailId);
    Task<Order?> CheckoutAsync(Guid orderId);
    Task<Order?> GetOrderAsync(Guid orderId);
    Task<List<Order>> GetAsync();
}
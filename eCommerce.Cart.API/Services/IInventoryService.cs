using eCommerce.Cart.API.Models.Dtos;

namespace eCommerce.Cart.API.Services;

public interface IInventoryService
{
    Task<InventoryDto?> UpdateQuantityByProductIdAsync(Guid productId, int quantity);
}
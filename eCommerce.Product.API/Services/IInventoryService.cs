using eCommerce.Product.API.Models.Dtos;

namespace eCommerce.Product.API.Services;

public interface IInventoryService
{
    Task<InventoryDto?> AddAsync(InventoryDto inventory);
    Task<List<InventoryDto>?> GetAsync();
    Task<InventoryDto?> GetByProductIdAsync(Guid productId);
    Task<InventoryDto?> AddQuantityByProductIdAsync(Guid productId, int quantity);
    Task DeleteAsync(Guid productId);
}
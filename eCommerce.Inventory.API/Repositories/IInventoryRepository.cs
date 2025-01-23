namespace eCommerce.Inventory.API.Repositories;

public interface IInventoryRepository
{
    Task<Models.Inventory> AddAsync(Models.Inventory inventory);
    Task<List<Models.Inventory>> GetAsync();
    Task<Models.Inventory?> GetAsync(Guid inventoryId);
    Task<Models.Inventory?> GetByProductIdAsync(Guid productId);
    Task<Models.Inventory?> AddQuantityByProductIdAsync(Guid productId, int quantity);
    Task<Models.Inventory?> UpdateQuantityByProductIdAsync(Guid productId, int quantity);
    Task DeleteAsync(Guid productId);
}
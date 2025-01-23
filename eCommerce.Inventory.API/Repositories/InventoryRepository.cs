using eCommerce.Inventory.API.Data;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Inventory.API.Repositories;

public class InventoryRepository : IInventoryRepository
{
    private readonly AppDbContext _db;

    public InventoryRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Models.Inventory> AddAsync(Models.Inventory inventory)
    {
        // Check if the inventory is already exist for the product
        var inventoryExist = await GetByProductIdAsync(inventory.ProductId);
        if (inventoryExist != null)
        {
            throw new Exception($"Inventory is already exist for Product id ({inventory.ProductId})");
        }

        inventory.CreatedOn = DateTime.Now;
        inventory.LastUpdatedOn = DateTime.Now;

        await _db.Inventories.AddAsync(inventory);
        await _db.SaveChangesAsync();

        return inventory;
    }

    public async Task<List<Models.Inventory>> GetAsync()
    {
        return await _db.Inventories.ToListAsync();
    }

    public async Task<Models.Inventory?> GetAsync(Guid inventoryId)
    {
        return await _db.FindAsync<Models.Inventory>(inventoryId);
    }

    public async Task<Models.Inventory?> GetByProductIdAsync(Guid productId)
    {
        return await _db.Inventories.FirstOrDefaultAsync(x => x.ProductId == productId);
    }

    public async Task<Models.Inventory?> AddQuantityByProductIdAsync(Guid productId, int quantity)
    {
        var inventory = await GetByProductIdAsync(productId) ??
            throw new KeyNotFoundException("Product id not found");

        inventory.Quantity += quantity;

        _db.Inventories.Update(inventory);
        await _db.SaveChangesAsync();

        return inventory;
    }

    public async Task<Models.Inventory?> UpdateQuantityByProductIdAsync(Guid productId, int quantity)
    {
        var inventory = await GetByProductIdAsync(productId) ??
            throw new KeyNotFoundException("Product id not found");
        
        if (inventory.Quantity > 0 && inventory.Quantity >= quantity)
        {
            inventory.Quantity -= quantity;

            _db.Inventories.Update(inventory);
            await _db.SaveChangesAsync();
        }
        else
        {
            throw new Exception("Product is out of stock or insufficient inventory available.");
        }

        return inventory;
    }

    public async Task DeleteAsync(Guid productId)
    {
        var inventory = await GetByProductIdAsync(productId) ?? throw new KeyNotFoundException("Inventory not found");

        _db.Inventories.Remove(inventory);
        await _db.SaveChangesAsync();
    }
}
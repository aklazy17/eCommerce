using eCommerce.Product.API.Data;
using eCommerce.Product.API.Models;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Product.API.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _db;

    public ProductRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Models.Product> AddAsync(Models.Product product)
    {
        // Check if the product name is already exist or not
        var productExist = await GetAsync(product.Name);
        if (productExist is not null)
        {
            throw new Exception($"{product.Name} product is already exist");
        }

        product.CreatedOn = DateTime.Now;
        product.LastUpdatedOn = DateTime.Now;

        await _db.Products.AddAsync(product);
        await _db.SaveChangesAsync();

        return product;
    }

    public async Task<Models.Product> UpdateAsync(Models.Product product)
    {
        // Check if the product name is already exist or not
        var productExist = await GetAsync(product.Name);
        if (productExist is not null && productExist.Id != product.Id)
        {
            throw new Exception($"{product.Name} product is already exist");
        }

        var p = await GetAsync(product.Id) ?? throw new KeyNotFoundException($"Product not found");

        p.Name = product.Name;
        p.Description = product.Description;
        p.LastUpdatedOn = DateTime.Now;

        _db.Products.Update(p);
        await _db.SaveChangesAsync();
        
        return p;
    }

    public async Task<Models.Product?> GetAsync(Guid id)
    {
        return await _db.FindAsync<Models.Product>(id);
    }

    public async Task<Models.Product?> GetAsync(string name)
    {
        return await _db.Products.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
    }

    public async Task<List<Models.Product>> GetAsync()
    {
        return await _db.Products.ToListAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var product = await GetAsync(id) ?? throw new KeyNotFoundException("Product not found");

        _db.Products.Remove(product);
        await _db.SaveChangesAsync();
    }
}
namespace eCommerce.Product.API.Repositories;

public interface IProductRepository
{
    Task<Models.Product> AddAsync(Models.Product product);
    Task<Models.Product> UpdateAsync(Models.Product product);
    Task<Models.Product?> GetAsync(Guid id);
    Task<Models.Product?> GetAsync(string name);
    Task<List<Models.Product>> GetAsync();
    Task DeleteAsync(Guid id);
}
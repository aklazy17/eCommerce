namespace eCommerce.ProductDetail.API.Repositories;

public interface IProductDetailRepository
{
    Task<List<Models.ProductDetail>> AddAsync(List<Models.ProductDetail> productDetails);
    Task<List<Models.ProductDetail>> UpdateAsync(List<Models.ProductDetail> productDetails);
    Task<Models.ProductDetail?> GetAsync(Guid id);
    Task<List<Models.ProductDetail>> GetAsync();
    Task<List<Models.ProductDetail>> GetByProductIdAsync(Guid productId);
    Task DeleteAsync(Guid productId);
}
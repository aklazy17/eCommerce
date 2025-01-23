using eCommerce.Product.API.Models.Dtos;

namespace eCommerce.Product.API.Services;

public interface IProductDetailService
{
    Task<List<ProductDetailDto>?> AddAsync(List<ProductDetailDto> productDetails);
    Task<List<ProductDetailDto>?> UpdateAsync(List<ProductDetailDto> productDetails);
    Task<List<ProductDetailDto>?> GetAsync();
    Task<List<ProductDetailDto>?> GetByProductIdAsync(Guid productId);
    Task DeleteAsync(Guid productId);
}
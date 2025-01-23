using eCommerce.Product.API.Models.Dtos;

namespace eCommerce.Product.API.Proxies;

public interface IProductProxy
{
    Task<ProductDto> AddAsync(AddProductRequestDto dto);
    Task<ProductDto> UpdateAsync(UpdateProductRequestDto dto);
    Task<ProductDto?> GetAsync(Guid id);
    Task<ProductDto?> GetAsync(string name);
    Task<List<ProductDto>> GetAsync();
    Task DeleteAsync(Guid id);
}
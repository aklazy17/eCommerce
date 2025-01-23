namespace eCommerce.Product.API.Models.Dtos;

public class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime? LastUpdatedOn { get; set; }
    public List<ProductDetailDto>? ProductDetails { get; set; }
    public InventoryDto? InventoryInfo { get; set; }
}
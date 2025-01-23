namespace eCommerce.Product.API.Models.Dtos;

public class ProductDetailDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public double Price { get; set; }
    public string Size { get; set; }
    public string? Design { get; set; }
}
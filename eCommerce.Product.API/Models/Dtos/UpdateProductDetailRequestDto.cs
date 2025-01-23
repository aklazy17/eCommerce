namespace eCommerce.Product.API.Models.Dtos;

public class UpdateProductDetailRequestDto
{
    public Guid Id { get; set; }
    public double Price { get; set; }
    public string Size { get; set; }
    public string? Design { get; set; }
}
namespace eCommerce.Product.API.Models.Dtos;

public class AddProductDetailRequestDto
{
    public double Price { get; set; }
    public string Size { get; set; }
    public string? Design { get; set; }
}
namespace eCommerce.ProductDetail.API.Models.Dtos;

public class UpdateProductDetailRequestDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public double Price { get; set; }
    public string Size { get; set; }
    public string? Design { get; set; }
}
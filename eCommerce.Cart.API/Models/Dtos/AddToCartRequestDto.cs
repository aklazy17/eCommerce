namespace eCommerce.Cart.API.Models.Dtos;

public class AddToCartRequestDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
}
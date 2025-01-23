namespace eCommerce.Cart.API.Models.Dtos;

public class OrderDto
{
    public Guid Id { get; set; }
    public double Total { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool CheckoutCompleted { get; set; }
    public List<OrderDetailDto> Details { get; set; }
}
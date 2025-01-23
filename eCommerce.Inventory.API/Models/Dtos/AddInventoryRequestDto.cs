namespace eCommerce.Inventory.API.Models.Dtos;

public class AddInventoryRequestDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
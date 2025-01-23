namespace eCommerce.Inventory.API.Models.Dtos;

public class InventoryDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public bool OutOfStock => Quantity <= 0;
    public DateTime? LastUpdatedOn { get; set; }
}
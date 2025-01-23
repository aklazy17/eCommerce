namespace eCommerce.Product.API.Models.Dtos;

public class InventoryDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public bool OutOfStock { get; set; }
    public DateTime? LastUpdatedOn { get; set; }
}
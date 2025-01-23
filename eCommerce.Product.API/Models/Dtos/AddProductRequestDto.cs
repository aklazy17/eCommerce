namespace eCommerce.Product.API.Models.Dtos
{
    public class AddProductRequestDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public List<AddProductDetailRequestDto>? ProductDetails { get; set; }
    }
}
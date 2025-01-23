namespace eCommerce.Product.API.Models.Dtos
{
    public class UpdateProductRequestDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<UpdateProductDetailRequestDto>? ProductDetails { get; set; }
    }
}
namespace eCommerce.Cart.API.Models.Dtos
{
    public class OrderDetailDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public Guid OrderId { get; set; }
    }
}
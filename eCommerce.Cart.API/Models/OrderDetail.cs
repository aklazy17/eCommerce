using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerce.Cart.API.Models;

public class OrderDetail
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
    public Guid OrderId { get; set; }

    [ForeignKey("OrderId")]
    public Order? Order { get; set; }
}
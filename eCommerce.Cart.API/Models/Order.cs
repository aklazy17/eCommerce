using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerce.Cart.API.Models;

public class Order
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    public double Total { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool CheckoutCompleted { get; set; }
    public List<OrderDetail> Details { get; set; }
}
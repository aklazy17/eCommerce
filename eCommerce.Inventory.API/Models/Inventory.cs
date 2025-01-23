using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace eCommerce.Inventory.API.Models;

public class Inventory
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public DateTime? CreatedOn { get; set; }
    public DateTime? LastUpdatedOn { get; set; }
}
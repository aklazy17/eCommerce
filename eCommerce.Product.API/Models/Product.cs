using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerce.Product.API.Models;

public class Product
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime? CreatedOn { get; set; }
    public DateTime? LastUpdatedOn { get; set; }
}
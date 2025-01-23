using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace eCommerce.ProductDetail.API.Models;

public class ProductDetail
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public Guid ProductId { get; set; }

    public double Price { get; set; }
    public string Size { get; set; }
    public string? Design { get; set; }
    public DateTime? CreatedOn { get; set; } = DateTime.Now;
    public DateTime? LastUpdatedOn { get; set; } = DateTime.Now;
}
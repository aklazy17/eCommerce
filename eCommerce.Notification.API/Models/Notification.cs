using eCommerce.Notification.API.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerce.Notification.API.Models;

public class Notification
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public EventType EventType {  get; set; }

    [Required]
    public string Message { get; set; }
    public string? Recipient { get; set; }
    public DateTime CreatedOn { get; set; }
}
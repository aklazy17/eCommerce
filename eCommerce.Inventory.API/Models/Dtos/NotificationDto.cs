using eCommerce.Inventory.API.Enums;

namespace eCommerce.Inventory.API.Models.Dtos;

public class NotificationDto
{
    public EventType EventType { get; set; }
    public string Message { get; set; }
    public string? Recipient { get; set; }
}
using eCommerce.Cart.API.Enums;

namespace eCommerce.Cart.API.Models.Dtos;

public class NotificationDto
{
    public EventType EventType { get; set; }
    public string Message { get; set; }
    public string? Recipient { get; set; }
}
using eCommerce.Product.API.Enums;

namespace eCommerce.Product.API.Models.Dtos;

public class NotificationDto
{
    public EventType EventType { get; set; }
    public string Message { get; set; }
    public string? Recipient { get; set; }
}
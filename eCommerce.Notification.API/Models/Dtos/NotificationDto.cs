using eCommerce.Notification.API.Enums;

namespace eCommerce.Notification.API.Models.Dtos;

public class NotificationDto
{
    public Guid Id { get; set; }
    public EventType EventType { get; set; }
    public string Message { get; set; }
    public string? Recipient { get; set; }
    public DateTime CreatedOn { get; set; }
}
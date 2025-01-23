using eCommerce.Notification.API.Enums;

namespace eCommerce.Notification.API.Models.Dtos
{
    public class AddNotificationRequestDto
    {
        public EventType EventType { get; set; }
        public string Message { get; set; }
        public string? Recipient { get; set; }
    }
}
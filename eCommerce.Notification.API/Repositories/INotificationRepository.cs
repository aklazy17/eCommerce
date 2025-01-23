namespace eCommerce.Notification.API.Repositories;

public interface INotificationRepository
{
    Task<List<Models.Notification>> GetAsync();
    Task<Models.Notification?> GetAsync(Guid id);
    Task<Models.Notification> AddAsync(Models.Notification notification);
}
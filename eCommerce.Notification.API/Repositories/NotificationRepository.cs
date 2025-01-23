using eCommerce.Notification.API.Data;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Notification.API.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly AppDbContext _db;
    private readonly ILogger<NotificationRepository> _logger;

    public NotificationRepository(AppDbContext db,
        ILogger<NotificationRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<Models.Notification> AddAsync(Models.Notification notification)
    {
        notification.CreatedOn = DateTime.Now;

        await _db.Notifications.AddAsync(notification);
        await _db.SaveChangesAsync();

        return notification;
    }

    public async Task<List<Models.Notification>> GetAsync()
    {
        return await _db.Notifications.ToListAsync();
    }

    public async Task<Models.Notification?> GetAsync(Guid id)
    {
        return await _db.FindAsync<Models.Notification>(id);
    }
}
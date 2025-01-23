using AutoMapper;
using eCommerce.Notification.API.Models.Dtos;

namespace eCommerce.Notification.API;

public class MappingConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        return new MapperConfiguration(config =>
        {
            config.CreateMap<NotificationDto, Models.Notification>();
            config.CreateMap<Models.Notification, NotificationDto>();
            config.CreateMap<AddNotificationRequestDto, Models.Notification>();
        });
    }
}
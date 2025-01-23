using AutoMapper;
using eCommerce.Cart.API.Models;
using eCommerce.Cart.API.Models.Dtos;

namespace eCommerce.Cart.API;

public class MappingConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        return new MapperConfiguration(config =>
        {
            config.CreateMap<OrderDto, Order>();
            config.CreateMap<Order, OrderDto>();

            config.CreateMap<OrderDetailDto, OrderDetail>();
            config.CreateMap<OrderDetail, OrderDetailDto>();
            config.CreateMap<AddToCartRequestDto, OrderDetail>();
        });
    }
}
using AutoMapper;
using eCommerce.Inventory.API.Models.Dtos;

namespace eCommerce.Inventory.API;

public class MappingConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        return new MapperConfiguration(config =>
        {
            config.CreateMap<InventoryDto, Models.Inventory>();
            config.CreateMap<Models.Inventory, InventoryDto>();
            config.CreateMap<AddInventoryRequestDto, Models.Inventory>();
        });
    }
}
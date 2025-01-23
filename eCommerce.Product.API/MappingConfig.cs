using AutoMapper;
using eCommerce.Product.API.Models.Dtos;

namespace eCommerce.Product.API;

public class MappingConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        return new MapperConfiguration(config =>
        {
            config.CreateMap<ProductDto, Models.Product>();
            config.CreateMap<Models.Product, ProductDto>();

            config.CreateMap<AddProductRequestDto, Models.Product>();
            config.CreateMap<UpdateProductRequestDto, Models.Product>();

            config.CreateMap<AddProductDetailRequestDto, ProductDetailDto>();
            config.CreateMap<UpdateProductDetailRequestDto, ProductDetailDto>();
        });
    }
}
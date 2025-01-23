using AutoMapper;
using eCommerce.ProductDetail.API.Models.Dtos;

namespace eCommerce.ProductDetail.API;

public class MappingConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        return new MapperConfiguration(config =>
        {
            config.CreateMap<ProductDetailDto, Models.ProductDetail>();
            config.CreateMap<Models.ProductDetail, ProductDetailDto>();
            config.CreateMap<AddProductDetailRequestDto, Models.ProductDetail>();
            config.CreateMap<UpdateProductDetailRequestDto, Models.ProductDetail>();
        });
    }
}
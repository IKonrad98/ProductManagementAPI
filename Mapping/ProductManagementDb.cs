using AutoMapper;
using ProductManagementAPI.DataAccess.Models;

namespace ProductManagementAPI.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ProductEntity, ProductModel>().ReverseMap();
        CreateMap<CreateProductModel, ProductEntity>();
        CreateMap<UpdateProductModel, ProductEntity>().ReverseMap()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<ProductEntity, UpdateProductModel>();
    }
}
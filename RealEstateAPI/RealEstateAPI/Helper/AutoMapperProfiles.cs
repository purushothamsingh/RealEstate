using AutoMapper;
using RealEstateAPI.DomainModels;
using RealEstateAPI.DomainModels.PropertyDtos;
using RealEstateAPI.Models.AuthModels;
using RealEstateAPI.Models.Property;

namespace RealEstateAPI.Helper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<PropertyDto, Property>().ReverseMap();

            CreateMap<Property, PropertyListDto>()
                .ForMember(d => d.City, opt => opt.MapFrom(src => src.City.Name))
                .ForMember(d => d.Country, opt => opt.MapFrom(src => src.City.Country))
                .ForMember(d => d.PropertyType, opt => opt.MapFrom(src => src.PropertyType.Name))
                .ForMember(d => d.FurnishingType, opt => opt.MapFrom(src => src.FurnishingType.Name))
                .ForMember(d => d.Photo, opt => opt.MapFrom(src => src.Photos
                        .FirstOrDefault(p => p.IsPrimary).ImageUrl));


            CreateMap<Property, PropertyDetailDto>()
               .ForMember(d => d.City, opt => opt.MapFrom(src => src.City.Name))
               .ForMember(d => d.Country, opt => opt.MapFrom(src => src.City.Country))
               .ForMember(d => d.PropertyType, opt => opt.MapFrom(src => src.PropertyType.Name))
               .ForMember(d => d.FurnishingType, opt => opt.MapFrom(src => src.FurnishingType.Name));

            CreateMap<PropertyType, KeyValuePairDto>().ReverseMap();

            CreateMap<FurnishingType, KeyValuePairDto>().ReverseMap();

            CreateMap<Db_Register, UserDto>().ReverseMap();

            CreateMap<Photo, PhotoDto>().ReverseMap();

        }
    }
}

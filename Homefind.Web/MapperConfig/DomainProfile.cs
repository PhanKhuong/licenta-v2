using System;
using System.Linq;
using AutoMapper;
using Homefind.Core.DomainModels;
using Homefind.Web.Models.PropertyViewModels;

namespace Homefind.Web.MapperConfig
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<SubmitPropertyModel, EstateFeature>();
            CreateMap<SubmitPropertyModel, EstateLocation>();
            CreateMap<SubmitPropertyModel, EstateUnit>()
                .ForMember(x => x.EstateFeature, opt => opt.MapFrom(s => s))
                .ForMember(x => x.EstateImages, opt => opt.Ignore())
                .ForMember(x => x.EstateLocation, opt => opt.MapFrom(s => s))
                .ForMember(x => x.EstateType, opt => opt.Ignore());

            CreateMap<EstateType, PropertyInfoModel>();
            CreateMap<EstateLocation, PropertyInfoModel>();
            CreateMap<EstateImage, PropertyInfoModel>();
            CreateMap<EstateUnit, PropertyInfoModel>()
                .ForMember(x => x.EstateTypeDes, opt => opt.MapFrom(src => src.EstateType.TypeName))
                .ForMember(x => x.City, opt => opt.MapFrom(src => src.EstateLocation.City))
                .ForMember(x => x.Address, opt => opt.MapFrom(src => src.EstateLocation.Address))
                .ForMember(x => x.AvatarImageId, opt => opt.MapFrom(src => src.EstateImages.First().Id));

            CreateMap<EstateUnit, FavouritesModel>();
            CreateMap<EstateLocation, FavouritesModel>();
            CreateMap<EstateImage, FavouritesModel>();
            CreateMap<Favourites, FavouritesModel>()
                .ForMember(x => x.Title, opt => opt.MapFrom(src => src.EstateUnit.Title))
                .ForMember(x => x.Price, opt => opt.MapFrom(src => src.EstateUnit.Price))
                .ForMember(x => x.City, opt => opt.MapFrom(src => src.EstateUnit.EstateLocation.City))
                .ForMember(x => x.AvatarImageId, opt => opt.MapFrom(src => src.EstateUnit.EstateImages.First().Id))
                .ForMember(x => x.Country, opt => opt.MapFrom(src => src.EstateUnit.EstateLocation.Country))
                .ForMember(x => x.Address, opt => opt.MapFrom(src => src.EstateUnit.EstateLocation.Address));

            CreateMap<Review, ReviewModel>()
                .ForMember(x => x.DateFormatted, opt => opt.MapFrom(src => String.Format("{0:ddd, MMM d, yyyy}", src.Date)))
                .ReverseMap();
        }
    }
}

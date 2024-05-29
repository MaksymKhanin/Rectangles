using Api.Business_Objects;
using Api.Entities;
using AutoMapper;

namespace Api.Mapping
{
    public class EntityToDomainMapping : Profile
    {
        public EntityToDomainMapping()
        {
            CreateMap<RectangleEntity, Rectangle>()
                .ForMember(dest => dest.Id, act => act.MapFrom(src => src.Id))
                .ForMember(dest => dest.X, act => act.MapFrom(src => src.X))
                .ForMember(dest => dest.Y, act => act.MapFrom(src => src.Y))
                .ForMember(dest => dest.Width, act => act.MapFrom(src => src.Width))
                .ForMember(dest => dest.Height, act => act.MapFrom(src => src.Height));
        }
    }
}

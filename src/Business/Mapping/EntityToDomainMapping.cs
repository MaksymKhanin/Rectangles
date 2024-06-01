using Api.Business_Objects;
using Api.Entities;
using AutoMapper;

namespace Api.Mapping
{
    public class EntityToDomainMapping : Profile
    {
        public EntityToDomainMapping()
        {
            CreateMap<RectangleEntity, Rectangle>();
        }
    }
}

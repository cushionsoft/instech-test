using AutoMapper;

namespace Claims
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Infrastructure.Entities.Claim, Core.Entities.Claim>().ReverseMap();
            CreateMap<Core.Entities.Claim, Web.Models.Claim>().ReverseMap();
            CreateMap<Infrastructure.Entities.Cover, Core.Entities.Cover>().ReverseMap();
            CreateMap<Core.Entities.Cover, Web.Models.Cover>().ReverseMap();
        }
    }
}

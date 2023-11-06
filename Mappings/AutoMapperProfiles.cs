using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Region, RegionDto>().ReverseMap();


            // When the variables that you want to map have different names
/*            CreateMap<UserDto, UserDomain>()
                .ForMember(x => x.Name, opt => opt.MapFrom(x => x.FullName))
                .ReverseMap();*/
        }
    }
}

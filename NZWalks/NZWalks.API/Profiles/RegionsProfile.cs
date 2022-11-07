using AutoMapper;

namespace NZWalks.API.Profiles
{
    public class RegionsProfile : Profile
    {
        public RegionsProfile()
        {
            CreateMap<Models.Domain.Region, Models.DTO.Region>()
                .ReverseMap();
            // .ForMember(dest => dest.Id,Options=>Options.MapFrom(src=>src.RegionId));
            // Suppose you want to assign region Id to Id field...
            // here we are having same name field ID to ID so no need to use this

            //.ReverseMap() reverse mapping also can do ..
            //suppose after reverse again we want data as it is so can use re- reverse
        }
    }
}

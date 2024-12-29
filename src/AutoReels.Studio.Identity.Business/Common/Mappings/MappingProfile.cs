using AutoMapper;
using AutoReels.Studio.Identity.Business.User.Commands;
using AutoReels.Studio.Identity.Common.Entities;

namespace AutoReels.Studio.Identity.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, CreateUserRequest>()
                .ForMember(d => d.FirstName, o => o.MapFrom(s => s.FirstName))
                .ForMember(d => d.LastName, o => o.MapFrom(s => s.LastName))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.Email))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.UserName));
        }
    }
}

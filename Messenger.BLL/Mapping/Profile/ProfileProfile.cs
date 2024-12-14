using AutoMapper;
using Messenger.BLL.DTO.Profile;

namespace Messenger.BLL.Mapping;

public class ProfileProfile : Profile
{
    public ProfileProfile()
    {
        CreateMap<Messenger.DAL.Entities.Profile, ProfileDto>();
        CreateMap<ProfileCreateDto, Messenger.DAL.Entities.Profile>();
        CreateMap<ProfileUpdateDto, Messenger.DAL.Entities.Profile>();
    }
}
using AutoMapper;
using Messenger.BLL.DTO.LastSeen;

namespace Messenger.BLL.Mapping.LastSeen;

public class LastSeenProfile : Profile
{
    public LastSeenProfile()
    {
        CreateMap<Messenger.DAL.Entities.Profile, LastSeenDto>()
            .ForMember(ls => ls.IsOnline,
                src =>
                    src.MapFrom(p => !p.LastSeen.HasValue));
    }
}
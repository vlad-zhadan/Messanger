using AutoMapper;
using Mesagger.BLL.DTO.LastSeen;

namespace Mesagger.BLL.Mapping.LastSeen;

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
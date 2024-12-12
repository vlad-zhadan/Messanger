using Mesagger.BLL.DTO.PersonalChat;
using Messenger.DAL.Entities;
using Profile = AutoMapper.Profile;

namespace Mesagger.BLL.Mapping.PersonalChat;

public class PersonalChatProfile : Profile
{
    public PersonalChatProfile()
    {
        CreateMap<Chat, PersonalChatDto>();
    }
}
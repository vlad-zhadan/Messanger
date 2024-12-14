using Messenger.BLL.DTO.PersonalChat;
using Messenger.DAL.Entities;
using Profile = AutoMapper.Profile;

namespace Messenger.BLL.Mapping.PersonalChat;

public class PersonalChatProfile : Profile
{
    public PersonalChatProfile()
    {
        CreateMap<Chat, PersonalChatDto>();
    }
}
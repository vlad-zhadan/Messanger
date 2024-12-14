using Messenger.BLL.DTO.GroupChat;
using Messenger.DAL.Entities;
using Profile = AutoMapper.Profile;

namespace Messenger.BLL.Mapping.GroupChat;

public class GroupChatProfile : Profile
{
    public GroupChatProfile()
    {
        CreateMap<Chat, GroupChatDto>();
    }
}
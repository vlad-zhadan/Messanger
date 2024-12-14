using Messenger.BLL.DTO.PersonalChatMessageDTO;
using Messenger.DAL.Entities;
using Profile = AutoMapper.Profile;

namespace Messenger.BLL.Mapping.PersonalMessage;

public class MessageProfile : Profile
{
    public MessageProfile()
    {
        CreateMap<Message, MessageReceiveDto>()
            .ForMember(pm => pm.UserOwnerId,
                opt => 
                    opt.MapFrom(src => src.MessageOwner.ProfileId))
            .ForMember(pm => pm.ChatId,
                opt => 
                    opt.MapFrom(src => src.MessageOwner.ChatId))
            .ForMember(pm => pm.ReceiverIds,
                opt => 
                    opt.MapFrom(src => src.Receivers.Select(r => r.UserReceiverId).ToList()));

        CreateMap<MessageSendDto, Message>();
    }
}
using Mesagger.BLL.DTO.PersonalChatMessageDTO;
using Messenger.DAL.Entities;
using Profile = AutoMapper.Profile;

namespace Mesagger.BLL.Mapping.PersonalMessage;

public class PersonalMessageProfile : Profile
{
    public PersonalMessageProfile()
    {
        CreateMap<Message, PersonalMessageDto>()
            .ForMember(pm => pm.UserOwnerId,
                opt => 
                    opt.MapFrom(src => src.MessageOwner.ProfileId))
            .ForMember(pm => pm.TimeRead,
                opt =>
                    opt.MapFrom(src => src.Receivers.Select(r => r.TimeRead).FirstOrDefault()) );

        CreateMap<PersonalMessageSendDto, Message>();
        CreateMap<Message, PersonalMessageReceiveDto>();
    }
}
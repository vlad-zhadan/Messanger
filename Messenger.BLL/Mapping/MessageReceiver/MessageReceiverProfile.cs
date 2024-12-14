using AutoMapper;
using Messenger.BLL.DTO.MessageReceiver;

namespace Messenger.BLL.Mapping.MessageReceiver;

public class MessageReceiverProfile : Profile
{
    public MessageReceiverProfile()
    {
        CreateMap<MessageReceiverSendDto, Messenger.DAL.Entities.MessageReceiver>()
            .ForMember(mr => mr.TimeRead, opt => opt.MapFrom(_ => DateTime.Now));
        CreateMap<Messenger.DAL.Entities.MessageReceiver, MessageReceiverReceiveDto>()
            .ForMember(mrd => mrd.ProfileReceiverId,
                src =>
                    src.MapFrom(mr => mr.UserReceiver.ProfileId));
    }
}
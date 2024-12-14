using AutoMapper;
using FluentResults;
using MediatR;
using Messenger.BLL.DTO.MessageReceiver;
using Messenger.BLL.MediatR.MessageReceiver.Create;
using Messenger.DAL.Repositories.Interfaces.Base;
using Microsoft.EntityFrameworkCore;

namespace Messenger.BLL.Mapping.MessageReceiver;

public class CreateMessageReceiverHandler : IRequestHandler<CreateMessageReceiverCommand, Result<MessageReceiverReceiveDto>>
{
    private readonly IRepositoryWrapper _wrapper;
    private readonly IMapper _mapper;

    public CreateMessageReceiverHandler(IRepositoryWrapper wrapper, IMapper mapper)
    {
        _wrapper = wrapper;
        _mapper = mapper;
    }
    public async Task<Result<MessageReceiverReceiveDto>> Handle(CreateMessageReceiverCommand request, CancellationToken cancellationToken)
    {
        var message = await 
            _wrapper.MessageRepository.GetFirstOrDefaultAsync(predicate: m =>
                    m.MessageId == request.messageReceiver.MessageId
                    && m.MessageOwner.ChatId == request.messageReceiver.ChatId,
                    include: source => source.Include(m => m.MessageOwner)
                );

        if (message is null)
        {
            var errorMessage = $"Message with id: {request.messageReceiver.MessageId} was not found";
            return Result.Fail(errorMessage);
        }

        var userOfChatThatReadTheMessage = await _wrapper.UserOfChatRepository.GetFirstOrDefaultAsync(predicate:
            uoc => uoc.ChatId == request.messageReceiver.ChatId
                   && uoc.ProfileId == request.messageReceiver.ProfileReceiverId
        );

        if (userOfChatThatReadTheMessage is null)
        {
            var errorMessage = $"Account of chat with id: {request.messageReceiver.ChatId} was not found";
            return Result.Fail(errorMessage);
        }
        
        var receiver = _mapper.Map<Messenger.DAL.Entities.MessageReceiver>(message);
        receiver.UserReceiverId = userOfChatThatReadTheMessage.UserOfChatId;
        
        var savedMessageReceiver = await _wrapper.MessageReceiverRepository.CreateAsync(receiver);
        await _wrapper.SaveChangesAsync();
        
        var receiverReceiveDto = _mapper.Map<MessageReceiverReceiveDto>(savedMessageReceiver);
        //receiverReceiveDto.ChatId = request.messageReceiver.ChatId;
        receiverReceiveDto.ProfileReceiverId = request.messageReceiver.ProfileReceiverId;
        
        return Result.Ok(receiverReceiveDto);

    }
}
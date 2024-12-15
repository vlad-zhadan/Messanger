using AutoMapper;
using FluentResults;
using MediatR;
using Mesagger.BLL.Security.Interface;
using Messenger.BLL.DTO.MessageReceiver;
using Messenger.BLL.MediatR.MessageReceiver.Create;
using Messenger.DAL.Repositories.Interfaces.Base;
using Microsoft.EntityFrameworkCore;

namespace Messenger.BLL.Mapping.MessageReceiver;

public class CreateMessageReceiverHandler : IRequestHandler<CreateMessageReceiverCommand, Result<MessageReceiverReceiveDto>>
{
    private readonly IRepositoryWrapper _wrapper;
    private readonly IMapper _mapper;
    private readonly IUserAccessor _userAccessor;

    public CreateMessageReceiverHandler(IRepositoryWrapper wrapper, IMapper mapper, IUserAccessor userAccessor)
    {
        _wrapper = wrapper;
        _mapper = mapper;
        _userAccessor = userAccessor;
    }
    public async Task<Result<MessageReceiverReceiveDto>> Handle(CreateMessageReceiverCommand request, CancellationToken cancellationToken)
    {
        var userId = _userAccessor.GetCurrentUserId();

        if (userId < 0)
        {
            var errorMessage = $"User {userId} not found";
            return Result.Fail(errorMessage);
        }
        
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
                   && uoc.ProfileId == userId
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
        receiverReceiveDto.ProfileReceiverId = userId;
        
        return Result.Ok(receiverReceiveDto);

    }
}
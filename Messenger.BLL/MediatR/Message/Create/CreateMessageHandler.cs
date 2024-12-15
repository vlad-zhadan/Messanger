using AutoMapper;
using FluentResults;
using MediatR;
using Mesagger.BLL.Security.Interface;
using Messenger.BLL.DTO.PersonalChatMessageDTO;
using Messenger.BLL.MediatR.PersonalMessage.Create;
using Messenger.DAL.Enums;
using Messenger.DAL.Repositories.Interfaces.Base;

namespace Messenger.BLL.MediatR.Message.Create;

public class CreateMessageHandler : IRequestHandler<CreateMessageCommand, Result<MessageReceiveDto>>
{
    private readonly IRepositoryWrapper _wrapper;
    private readonly IMapper _mapper;
    private readonly IUserAccessor _userAccessor;

    public CreateMessageHandler(IRepositoryWrapper wrapper, IMapper mapper, IUserAccessor userAccessor)
    {
        _wrapper = wrapper;
        _mapper = mapper;
        _userAccessor = userAccessor;
    }
    
    public async Task<Result<MessageReceiveDto>> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
    {
        var userId = _userAccessor.GetCurrentUserId();

        if (userId < 0)
        {
            var errorMessage = $"User {userId} not found";
            return Result.Fail(errorMessage);
        }
        
        var userOfChat = await _wrapper.UserOfChatRepository
            .GetFirstOrDefaultAsync(predicate: 
                uoc => 
                    uoc.ChatId == request.NewMessage.ChatId 
                    && uoc.ProfileId == userId
                       );

        if (userOfChat is null)
        {
            var errorMessage = $"Account of chat with id {request.NewMessage.ChatId} was not found.";
            return Result.Fail(errorMessage);
        }
        
        if (userOfChat.Status is ChatStatus.Blocked )
        {
            var errorMessage = $"Account of chat with id {request.NewMessage.ChatId} is blocked from the chat.";
            return Result.Fail(errorMessage);
        }
        
        if (userOfChat.Status is ChatStatus.Blocking )
        {
            var errorMessage = $"Account of chat with id {request.NewMessage.ChatId} is bloking another user.";
            return Result.Fail(errorMessage);
        }

        if (userOfChat.Role is ChatRole.Reader)
        {
            var errorMessage = $"Account of chat with id {request.NewMessage.ChatId} does not have the permission to write in the chat.";
            return Result.Fail(errorMessage);
        }
        
        var chat = await _wrapper.ChatRepository.GetFirstOrDefaultAsync(predicate: c => c.ChatId == userOfChat.ChatId);

        if (chat is null)
        {
            var errorMessage = $"Chat with id {userOfChat.ChatId} was not found.";
            return Result.Fail(errorMessage);
        }
        
        try
        {
            var newMessage = _mapper.Map<Messenger.DAL.Entities.Message>(request.NewMessage);
            newMessage.MessageOwnerId = userOfChat.UserOfChatId;
            newMessage.TimeSent = DateTime.Now;
            newMessage.Status = MessageStatus.Normal;

            var createdMessage = await _wrapper.MessageRepository.CreateAsync(newMessage);
            await _wrapper.SaveChangesAsync();
            
            chat.LastMessageId = createdMessage.MessageId;
            _wrapper.ChatRepository.Update(chat);
            await _wrapper.SaveChangesAsync();

            var createdMessageDto = _mapper.Map<MessageReceiveDto>(createdMessage);
            createdMessageDto.ChatId = request.NewMessage.ChatId;
            createdMessageDto.UserOwnerId = userId;

            return Result.Ok(createdMessageDto);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
        
    }
}
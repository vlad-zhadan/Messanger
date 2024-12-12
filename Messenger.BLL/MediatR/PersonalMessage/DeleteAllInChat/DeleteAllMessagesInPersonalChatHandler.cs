using FluentResults;
using MediatR;
using Messenger.DAL.Enums;
using Messenger.DAL.Repositories.Interfaces.Base;

namespace Mesagger.BLL.MediatR.PersonalMessage;

public class DeleteAllMessagesInPersonalChatHandler : IRequestHandler<DeleteAllMessagesInPersonalChatCommand, Result<int>>
{
    private readonly IRepositoryWrapper _wrapper;

    public DeleteAllMessagesInPersonalChatHandler(IRepositoryWrapper wrapper)
    {
        _wrapper = wrapper;
    }
    
    public async Task<Result<int>> Handle(DeleteAllMessagesInPersonalChatCommand request, CancellationToken cancellationToken)
    {
        var chatToDeleteMessages = await _wrapper.ChatRepository.GetFirstOrDefaultAsync(predicate: c => c.ChatId == request.ChatId);

        if (chatToDeleteMessages is null)
        {
            var errorMessage = $"Chat with ID {request.ChatId} does not exist.";
            return Result.Fail(errorMessage);
        }

        var usersOfChat = await _wrapper.UserOfChatRepository.GetAllAsync(predicate: c => c.ChatId == request.ChatId);
        
        
        // To DO need to check if has the permition as Admin
        // if (usersOfChat.Role == ChatRole.Admin)
        // {
        //     
        // }
        
        
        if (chatToDeleteMessages.Type == ChatType.PersonalChat)
        {
            
        }
        
        foreach (var userOfChat in usersOfChat)
        {
            _wrapper.MessageRepository.Delete(predicate: m => m.MessageOwner == userOfChat);
        }

        return Result.Ok(request.ChatId);
    }
}
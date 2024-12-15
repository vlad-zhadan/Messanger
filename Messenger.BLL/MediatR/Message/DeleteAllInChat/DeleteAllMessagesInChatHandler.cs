using FluentResults;
using MediatR;
using Mesagger.BLL.Security.Interface;
using Messenger.DAL.Enums;
using Messenger.DAL.Repositories.Interfaces.Base;

namespace Messenger.BLL.MediatR.PersonalMessage;

public class DeleteAllMessagesInChatHandler : IRequestHandler<DeleteAllMessagesInChatCommand, Result<int>>
{
    private readonly IRepositoryWrapper _wrapper;
    private readonly IUserAccessor _userAccessor;

    public DeleteAllMessagesInChatHandler(IRepositoryWrapper wrapper, IUserAccessor userAccessor)
    {
        _wrapper = wrapper;
        _userAccessor = userAccessor;
    }
    
    public async Task<Result<int>> Handle(DeleteAllMessagesInChatCommand request, CancellationToken cancellationToken)
    {
        var userId = _userAccessor.GetCurrentUserId();

        if (userId < 0)
        {
            var errorMessage = $"User {userId} not found";
            return Result.Fail(errorMessage);
        }
        
        var chatToDeleteMessages = await _wrapper.ChatRepository.GetFirstOrDefaultAsync(predicate: c => c.ChatId == request.ChatId);

        if (chatToDeleteMessages is null)
        {
            var errorMessage = $"Chat with ID {request.ChatId} does not exist.";
            return Result.Fail(errorMessage);
        }
        
        var userOfChatWhoWantToDeleteAllMessages = await _wrapper.UserOfChatRepository.GetFirstOrDefaultAsync(
            predicate:uoc => uoc.ChatId == chatToDeleteMessages.ChatId
            && uoc.ProfileId == userId
            );

        if (userOfChatWhoWantToDeleteAllMessages is null)
        {
            var errorMessage = $"User  with ID {userId} does not exist in the chat.";
            return Result.Fail(errorMessage);
        }
        
        if (userOfChatWhoWantToDeleteAllMessages.Role is not ChatRole.Admin)
        {
            var errorMessage = $"User with ID {userId} does not have an administrator role.";
            return Result.Fail(errorMessage);
        }
        
        var usersOfChat = await _wrapper.UserOfChatRepository.GetAllAsync(predicate: c => c.ChatId == request.ChatId);
        
        foreach (var userOfChat in usersOfChat)
        {
            _wrapper.MessageRepository.Delete(predicate: m => m.MessageOwner == userOfChat);
        }

        return Result.Ok(request.ChatId);
    }
}
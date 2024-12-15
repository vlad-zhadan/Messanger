using AutoMapper;
using FluentResults;
using MediatR;
using Mesagger.BLL.Security.Interface;
using Messenger.BLL.MediatR.PersonalMessage;
using Messenger.DAL.Enums;
using Messenger.DAL.Repositories.Interfaces.Base;

namespace Messenger.BLL.MediatR.PersonalChat.Delete;

public class DeletePersonalChatHandler : IRequestHandler<DeletePersonalChatCommand, Result<int>>
{
    private readonly IRepositoryWrapper _wrapper;
    private readonly IUserAccessor _userAccessor;
    private readonly IMediator _mediator;

    public DeletePersonalChatHandler(IRepositoryWrapper wrapper, IUserAccessor userAccessor, IMediator mediator)
    {
        _wrapper = wrapper;
        _userAccessor = userAccessor;
        _mediator = mediator;
    }
    
    public async Task<Result<int>> Handle(DeletePersonalChatCommand request, CancellationToken cancellationToken)
    {
        var userId = _userAccessor.GetCurrentUserId();

        if (userId < 0)
        {
            var errorMessage = $"User {userId} not found";
            return Result.Fail(errorMessage);
        }
        
        var chatToDelete = await _wrapper.ChatRepository.GetFirstOrDefaultAsync(predicate: c => c.ChatId == request.ChatId);

        if (chatToDelete is null)
        {
            var errorMessage = $"Chat with ID {request.ChatId} does not exist.";
            return Result.Fail(errorMessage);
        }
        
        var userOfChatWhoWantToDeleteTheChat = await _wrapper.UserOfChatRepository.GetFirstOrDefaultAsync(
            predicate: c => c.ChatId == request.ChatId
            && c.ProfileId == userId
            );

        if (userOfChatWhoWantToDeleteTheChat is null)
        {
            var errorMessage = $"User {userId} is not in chat with ID {request.ChatId}.";
            return Result.Fail(errorMessage);
        }

        if (userOfChatWhoWantToDeleteTheChat.Status is ChatStatus.Blocked && chatToDelete.Type is not ChatType.PersonalChat)
        {
            var errorMessage = $"User {userId} is blocked.";
            return Result.Fail(errorMessage);
        }

        if (userOfChatWhoWantToDeleteTheChat.Role is not ChatRole.Admin)
        {
            var errorMessage = $"User {userId} does not have an administrator role.";
            return Result.Fail(errorMessage);
        }

        var resultOfDeletingMessages = await _mediator.Send(new DeleteAllMessagesInChatCommand(chatToDelete.ChatId));

        if (resultOfDeletingMessages.IsFailed)
        {
            var errorMessage = "Could not delete all chat messages";
            return Result.Fail(errorMessage);
        }
        
        chatToDelete.DeletedAt = DateTime.Now;
        _wrapper.ChatRepository.Update(chatToDelete);
        await _wrapper.SaveChangesAsync();

        return Result.Ok(chatToDelete.ChatId);
    }
}
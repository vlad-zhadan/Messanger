using FluentResults;
using MediatR;
using Mesagger.BLL.Security.Interface;
using Messenger.DAL.Enums;
using Messenger.DAL.Repositories.Interfaces.Base;
using Microsoft.EntityFrameworkCore;

namespace Mesagger.BLL.MediatR.Message.Delete;

public class DeleteMessageHandler : IRequestHandler<DeleteMessageQuery, Result<int>>
{
    private readonly IRepositoryWrapper _wrapper;
    private readonly IUserAccessor _userAccessor;

    public DeleteMessageHandler(IRepositoryWrapper wrapper, IUserAccessor userAccessor)
    {
        _wrapper = wrapper;
        _userAccessor = userAccessor;
    }

    public async Task<Result<int>> Handle(DeleteMessageQuery request, CancellationToken cancellationToken)
    {
        var userId = _userAccessor.GetCurrentUserId();

        if (userId < 0)
        {
            var errorMessage = $"User {userId} not found";
            return Result.Fail(errorMessage);
        }
        
        var message = await _wrapper.MessageRepository.GetFirstOrDefaultAsync(
            predicate: m => m.MessageId == request.MessageId,
            include: source => source.Include(m => m.MessageOwner));

        if (message is null)
        {
            var errorMessage = $"Message with ID {request.MessageId} does not exist.";
            return Result.Fail(errorMessage);
        }

        if (message.MessageOwner.ProfileId != userId)
        {
            var userWhoDelete = await _wrapper.UserOfChatRepository.GetFirstOrDefaultAsync(
                predicate: uoc => uoc.ChatId == message.MessageOwner.ChatId || uoc.ProfileId == userId
            );

            
            if (userWhoDelete is null)
            {
                var errorMessage = $"Account with ID {userId} does not exist in this chat.";
                return Result.Fail(errorMessage);
            }
            
            if (userWhoDelete.Status is ChatStatus.Blocked)
            {
                var errorMessage = $"Account with ID {userId} blocked.";
                return Result.Fail(errorMessage);
            }

            if (userWhoDelete.Role is not ChatRole.Admin)
            {
                var errorMessage = $"Account with ID {userId} does not have an admin role.";
                return Result.Fail(errorMessage);
            }
            
        }
        
        _wrapper.MessageRepository.Delete(message);
        await _wrapper.SaveChangesAsync();
        
        return Result.Ok(message.MessageOwner.ChatId);
    }
}
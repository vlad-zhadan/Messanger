using FluentResults;
using MediatR;
using Mesagger.BLL.Security.Interface;
using Messenger.BLL.MediatR.PersonalChat.Create;
using Messenger.DAL.Enums;
using Messenger.DAL.Repositories.Interfaces.Base;
using Microsoft.EntityFrameworkCore;

namespace Mesagger.BLL.MediatR.PersonalChat.Block;

public class BlockPersonalChatHandler : IRequestHandler<BlockPersonalChatCommand, Result<int>>
{
    private readonly IRepositoryWrapper _wrapper;
    private readonly IUserAccessor _userAccessor;
    private readonly IMediator _mediator;

    public BlockPersonalChatHandler(IRepositoryWrapper wrapper, IUserAccessor userAccessor, IMediator mediator)
    {
        _wrapper = wrapper;
        _userAccessor = userAccessor;
        _mediator = mediator;
    }
    
    public async Task<Result<int>> Handle(BlockPersonalChatCommand request, CancellationToken cancellationToken)
    {
        var userId = _userAccessor.GetCurrentUserId();

        if (userId < 0)
        {
            var errorMessage = $"User {userId} not found";
            return Result.Fail(errorMessage);
        }
        
        var personalChat =
            await _wrapper.ChatRepository.GetPersonalOrDefaultChatAsync(userId,
                request.UserToBlockId);

        if (personalChat is null)
        {
            var createdChatDto = await _mediator.Send(new CreatePersonalChatCommand(request.UserToBlockId));

            if (createdChatDto.IsFailed)
            {
                var errorMessage = $"Chat was not created";
                return Result.Fail(errorMessage);
            }
            
            personalChat = await
                _wrapper.ChatRepository.GetFirstOrDefaultAsync(predicate: c => c.ChatId == createdChatDto.Value.ChatId);
            
            if (personalChat is null)
            {
                var errorMessage = $"Chat was not created";
                return Result.Fail(errorMessage);
            }
        }

        var userOfChatWhoIsBlocking = await _wrapper.UserOfChatRepository.GetFirstOrDefaultAsync(
            predicate: uoc => uoc.ChatId == personalChat.ChatId
                              && uoc.ProfileId == userId
        );
        
        var userOfChatWhoIsBlocked = await _wrapper.UserOfChatRepository.GetFirstOrDefaultAsync(
            predicate: uoc => uoc.ChatId == personalChat.ChatId
                              && uoc.ProfileId == request.UserToBlockId
        );

        if (userOfChatWhoIsBlocking is null || userOfChatWhoIsBlocked is null )
        {
            var errorMessage = $"No personal chat with two person was found.";
            return Result.Fail(errorMessage);
        }

        if (userOfChatWhoIsBlocked.Status is ChatStatus.Blocked)
        {
            var errorMessage = $"User is already being blocked.";
            return Result.Fail(errorMessage);
        }
        
        if (userOfChatWhoIsBlocked.Status is ChatStatus.Blocking)
        {
            var errorMessage = $"User is already blocking you.";
            return Result.Fail(errorMessage);
        }

        userOfChatWhoIsBlocking.Status = ChatStatus.Blocking;
        userOfChatWhoIsBlocked.Status = ChatStatus.Blocked;
        
        _wrapper.UserOfChatRepository.Update(userOfChatWhoIsBlocking);
        _wrapper.UserOfChatRepository.Update(userOfChatWhoIsBlocked);

        await _wrapper.SaveChangesAsync();
        
        return Result.Ok(personalChat.ChatId);
    }
}
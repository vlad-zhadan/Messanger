using FluentResults;
using MediatR;
using Messenger.DAL.Enums;
using Messenger.DAL.Repositories.Interfaces.Base;
using Microsoft.EntityFrameworkCore;

namespace Mesagger.BLL.MediatR.PersonalChat.Block;

public class BlockPersonalChatHandler : IRequestHandler<BlockPersonalChatCommand, Result<int>>
{
    private readonly IRepositoryWrapper _wrapper;

    public BlockPersonalChatHandler(IRepositoryWrapper wrapper)
    {
        _wrapper = wrapper;
    }
    
    public async Task<Result<int>> Handle(BlockPersonalChatCommand request, CancellationToken cancellationToken)
    {
        var personalChat =
            await _wrapper.ChatRepository.GetPersonalOrDefaultChatAsync(request.UserId,
                request.UserToBlockId);

        if (personalChat is null)
        {
            var errorMessage = $"No personal chat with two person was found.";
            return Result.Fail(errorMessage);
        }

        var userOfChatWhoIsBlocking = await _wrapper.UserOfChatRepository.GetFirstOrDefaultAsync(
            predicate: uoc => uoc.ChatId == personalChat.ChatId
                              && uoc.ProfileId == request.UserId
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

        userOfChatWhoIsBlocking.Status = ChatStatus.Blocking;
        userOfChatWhoIsBlocked.Status = ChatStatus.Blocked;
        
        _wrapper.UserOfChatRepository.Update(userOfChatWhoIsBlocking);
        _wrapper.UserOfChatRepository.Update(userOfChatWhoIsBlocked);

        await _wrapper.SaveChangesAsync();
        
        return Result.Ok(personalChat.ChatId);
    }
}
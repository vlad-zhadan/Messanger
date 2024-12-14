using FluentResults;
using MediatR;
using Messenger.DAL.Enums;
using Messenger.DAL.Repositories.Interfaces.Base;
using Microsoft.EntityFrameworkCore;

namespace Mesagger.BLL.MediatR.Message.Delete;

public class DeleteMessageHandler : IRequestHandler<DeleteMessageQuery, Result<int>>
{
    private readonly IRepositoryWrapper _wrapper;

    public DeleteMessageHandler(IRepositoryWrapper wrapper)
    {
        _wrapper = wrapper;
    }

    public async Task<Result<int>> Handle(DeleteMessageQuery request, CancellationToken cancellationToken)
    {
        var message = await _wrapper.MessageRepository.GetFirstOrDefaultAsync(
            predicate: m => m.MessageId == request.MessageId,
            include: source => source.Include(m => m.MessageOwner));

        if (message is null)
        {
            var errorMessage = $"Message with ID {request.MessageId} does not exist.";
            return Result.Fail(errorMessage);
        }

        if (message.MessageOwner.ProfileId != request.UserId)
        {
            var userWhoDelete = await _wrapper.UserOfChatRepository.GetFirstOrDefaultAsync(
                predicate: uoc => uoc.ChatId == message.MessageOwner.ChatId || uoc.ProfileId == request.UserId
            );

            if (userWhoDelete is null)
            {
                var errorMessage = $"User with ID {request.UserId} does not exist.";
                return Result.Fail(errorMessage);
            }

            if (userWhoDelete.Role != ChatRole.Admin)
            {
                var errorMessage = $"User with ID {request.UserId} does not have an admin role.";
                return Result.Fail(errorMessage);
            }
            
        }
        
        _wrapper.MessageRepository.Delete(message);
        await _wrapper.SaveChangesAsync();
        
        return Result.Ok(message.MessageOwner.ChatId);
    }
}
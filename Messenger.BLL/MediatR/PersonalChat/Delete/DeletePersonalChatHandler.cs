using AutoMapper;
using FluentResults;
using MediatR;
using Messenger.DAL.Repositories.Interfaces.Base;

namespace Messenger.BLL.MediatR.PersonalChat.Delete;

public class DeletePersonalChatHandler : IRequestHandler<DeletePersonalChatCommand, Result<int>>
{
    private readonly IRepositoryWrapper _wrapper;

    public DeletePersonalChatHandler(IRepositoryWrapper wrapper)
    {
        _wrapper = wrapper;
    }
    
    public async Task<Result<int>> Handle(DeletePersonalChatCommand request, CancellationToken cancellationToken)
    {
        var chatToDelete = await _wrapper.ChatRepository.GetFirstOrDefaultAsync(predicate: c => c.ChatId == request.ChatId);

        if (chatToDelete is null)
        {
            var errorMessage = $"Chat with ID {request.ChatId} does not exist.";
            return Result.Fail(errorMessage);
        }
        
        _wrapper.ChatRepository.Delete(chatToDelete);
        await _wrapper.SaveChangesAsync();

        return Result.Ok(chatToDelete.ChatId);
    }
}
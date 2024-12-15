using AutoMapper;
using FluentResults;
using MediatR;
using Mesagger.BLL.Security.Interface;
using Messenger.BLL.DTO.PersonalChatMessageDTO;
using Messenger.DAL.Enums;
using Messenger.DAL.Repositories.Interfaces.Base;
using Microsoft.EntityFrameworkCore;

namespace Mesagger.BLL.MediatR.Message.Edit;

public class EditMessageHandler : IRequestHandler<EditMessageQuery, Result<MessageReceiveDto>>
{
    private readonly IRepositoryWrapper _wrapper;
    private readonly IMapper _mapper;
    private readonly IUserAccessor _userAccessor;

    public EditMessageHandler(IRepositoryWrapper wrapper, IMapper mapper, IUserAccessor userAccessor)
    {
        _wrapper = wrapper;
        _mapper = mapper;
        _userAccessor = userAccessor;
    }
    public async Task<Result<MessageReceiveDto>> Handle(EditMessageQuery request, CancellationToken cancellationToken)
    {
        var userId = _userAccessor.GetCurrentUserId();

        if (userId < 0)
        {
            var errorMessage = $"User {userId} not found";
            return Result.Fail(errorMessage);
        }
        
        var message = await _wrapper.MessageRepository.GetFirstOrDefaultAsync(
            predicate: m => m.MessageId == request.MessageToEdit.MessageId,
            include: source => source.Include(m => m.MessageOwner));

        if (message is null)
        {
            var errorMessage = $"Message with ID {request.MessageToEdit.MessageId} does not exist.";
            return Result.Fail(errorMessage);
        }

        if (message.MessageOwner.ProfileId != userId)
        {
                var errorMessage = $"Account with ID {userId} does not have a permission to edit because it does belong to {message.MessageOwner.ProfileId}.";
                return Result.Fail(errorMessage);
        }

        message.Text = request.MessageToEdit.NewText;
        _wrapper.MessageRepository.Update(message);
        await _wrapper.SaveChangesAsync();
        
        var editedMessage = await _wrapper.MessageRepository.GetFirstOrDefaultAsync(
            predicate: m => m.MessageId == request.MessageToEdit.MessageId,
            include: source => source
                .Include(m => m.MessageOwner)
                .Include(m => m.Receivers)
            );
        
        var editedMessageDto = _mapper.Map<MessageReceiveDto>(editedMessage);
        
        return Result.Ok(editedMessageDto);
    }
}
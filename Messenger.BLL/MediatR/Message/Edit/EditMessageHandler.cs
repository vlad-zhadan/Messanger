using AutoMapper;
using FluentResults;
using MediatR;
using Messenger.BLL.DTO.PersonalChatMessageDTO;
using Messenger.DAL.Enums;
using Messenger.DAL.Repositories.Interfaces.Base;
using Microsoft.EntityFrameworkCore;

namespace Mesagger.BLL.MediatR.Message.Edit;

public class EditMessageHandler : IRequestHandler<EditMessageQuery, Result<MessageReceiveDto>>
{
    private readonly IRepositoryWrapper _wrapper;
    private readonly IMapper _mapper;

    public EditMessageHandler(IRepositoryWrapper wrapper, IMapper mapper)
    {
        _wrapper = wrapper;
        _mapper = mapper;
    }
    public async Task<Result<MessageReceiveDto>> Handle(EditMessageQuery request, CancellationToken cancellationToken)
    {
        var message = await _wrapper.MessageRepository.GetFirstOrDefaultAsync(
            predicate: m => m.MessageId == request.MessageToEdit.MessageId,
            include: source => source.Include(m => m.MessageOwner));

        if (message is null)
        {
            var errorMessage = $"Message with ID {request.MessageToEdit.MessageId} does not exist.";
            return Result.Fail(errorMessage);
        }

        if (message.MessageOwner.ProfileId != request.UserId)
        {
            var userWhoDelete = await _wrapper.UserOfChatRepository.GetFirstOrDefaultAsync(
                predicate: uoc => uoc.ChatId == message.MessageOwner.ChatId || uoc.ProfileId == request.UserId
            );

            if (userWhoDelete is null)
            {
                var errorMessage = $"Account with ID {request.UserId} does not exist.";
                return Result.Fail(errorMessage);
            }

            if (userWhoDelete.Role != ChatRole.Admin)
            {
                var errorMessage = $"Account with ID {request.UserId} does not have an admin role.";
                return Result.Fail(errorMessage);
            }
            
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
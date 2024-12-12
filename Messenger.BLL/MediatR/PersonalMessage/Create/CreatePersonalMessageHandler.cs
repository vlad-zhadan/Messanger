using AutoMapper;
using FluentResults;
using MediatR;
using Mesagger.BLL.DTO.PersonalChatMessageDTO;
using Messenger.DAL.Entities;
using Messenger.DAL.Enums;
using Messenger.DAL.Repositories.Interfaces.Base;

namespace Mesagger.BLL.MediatR.PersonalMessage.Create;

public class CreatePersonalMessageHandler : IRequestHandler<CreatePersonalMessageCommand, Result<PersonalMessageReceiveDto>>
{
    private readonly IRepositoryWrapper _wrapper;
    private readonly IMapper _mapper;

    public CreatePersonalMessageHandler(IRepositoryWrapper wrapper, IMapper mapper)
    {
        _wrapper = wrapper;
        _mapper = mapper;
    }
    
    public async Task<Result<PersonalMessageReceiveDto>> Handle(CreatePersonalMessageCommand request, CancellationToken cancellationToken)
    {
        var userOfChat = await _wrapper.UserOfChatRepository
            .GetFirstOrDefaultAsync(predicate: 
                uoc => 
                    uoc.ChatId == request.NewMessage.ChatId 
                    && uoc.ProfileId == request.NewMessage.UserOwnerId
                       );

        if (userOfChat is null)
        {
            var errorMessage = $"User of chat with id {request.NewMessage.ChatId} was not found.";
            return Result.Fail(errorMessage);
        }

        try
        {
            var newMessage = _mapper.Map<Message>(request.NewMessage);
            newMessage.MessageOwnerId = userOfChat.UserOfChatId;
            newMessage.TimeSent = DateTime.Now;
            newMessage.Status = MessageStatus.Normal;

            var createdMessage = await _wrapper.MessageRepository.CreateAsync(newMessage);
            await _wrapper.SaveChangesAsync();

            var createdMessageDto = _mapper.Map<PersonalMessageReceiveDto>(createdMessage);
            createdMessageDto.ChatId = request.NewMessage.ChatId;
            createdMessageDto.UserMessageOwnerId = request.NewMessage.UserOwnerId;

            return Result.Ok(createdMessageDto);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
        
    }
}
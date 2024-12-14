using AutoMapper;
using FluentResults;
using MediatR;
using Messenger.BLL.DTO.PersonalChat;
using Messenger.DAL.Entities;
using Messenger.DAL.Enums;
using Messenger.DAL.Repositories.Interfaces.Base;

namespace Messenger.BLL.MediatR.PersonalChat.Create;

public class CreatePersonalChatHandler : IRequestHandler<CreatePersonalChatCommand, Result<PersonalChatDto>>
{
    private readonly IRepositoryWrapper _wrapper;
    private readonly IMapper _mapper;

    public CreatePersonalChatHandler(IRepositoryWrapper wrapper, IMapper mapper)
    {
        _wrapper = wrapper;
        _mapper = mapper;
    }
    public async Task<Result<PersonalChatDto>> Handle(CreatePersonalChatCommand request, CancellationToken cancellationToken)
    {
        var sharedСhat =
            await _wrapper.ChatRepository.GetPersonalOrDefaultChatAsync(request.ChatUsers.FirstUserId,
                request.ChatUsers.SecondUserId);

        //need to add validation for blocking
        if (sharedСhat is not null)
        {
            var errorMessage = "Personal chat is already in use.";
            return Result.Fail(errorMessage);
        }

        var newPersonalChat = new Chat()
        {
            Type = ChatType.PersonalChat,
            MaxParticipants = 2,
            IsPrivate = true,
            CreatedAt = DateTime.Now,
        };
        try
        {
            var createdPersonalChat = await _wrapper.ChatRepository.CreateAsync(newPersonalChat);
            await _wrapper.SaveChangesAsync();

            var newUserOfChatFirst = new UserOfChat()
            {
                ChatId = createdPersonalChat.ChatId,
                ProfileId = request.ChatUsers.FirstUserId,
                Role = ChatRole.Admin,
                Status = ChatStatus.Ok,
            };

            var newUserOfChatSecond = new UserOfChat()
            {
                ChatId = createdPersonalChat.ChatId,
                ProfileId = request.ChatUsers.SecondUserId,
                Role = ChatRole.Admin,
                Status = ChatStatus.Ok,
            };

            await _wrapper.UserOfChatRepository.CreateAsync(newUserOfChatFirst);
            await _wrapper.UserOfChatRepository.CreateAsync(newUserOfChatSecond);
            await _wrapper.SaveChangesAsync();

            var createdPersonalChatDto = _mapper.Map<PersonalChatDto>(createdPersonalChat);
            createdPersonalChatDto.SecondUserId = request.ChatUsers.SecondUserId;

            return Result.Ok(createdPersonalChatDto);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
        
    }
}
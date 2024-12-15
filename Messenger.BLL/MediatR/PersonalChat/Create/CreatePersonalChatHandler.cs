using AutoMapper;
using FluentResults;
using MediatR;
using Mesagger.BLL.Security.Interface;
using Messenger.BLL.DTO.PersonalChat;
using Messenger.DAL.Entities;
using Messenger.DAL.Enums;
using Messenger.DAL.Repositories.Interfaces.Base;

namespace Messenger.BLL.MediatR.PersonalChat.Create;

public class CreatePersonalChatHandler : IRequestHandler<CreatePersonalChatCommand, Result<PersonalChatDto>>
{
    private readonly IRepositoryWrapper _wrapper;
    private readonly IMapper _mapper;
    private readonly IUserAccessor _userAccessor;

    public CreatePersonalChatHandler(IRepositoryWrapper wrapper, IMapper mapper, IUserAccessor userAccessor)
    {
        _wrapper = wrapper;
        _mapper = mapper;
        _userAccessor = userAccessor;
    }
    public async Task<Result<PersonalChatDto>> Handle(CreatePersonalChatCommand request, CancellationToken cancellationToken)
    {
        var userId = _userAccessor.GetCurrentUserId();

        if (userId < 0)
        {
            var errorMessage = $"User {userId} not found";
            return Result.Fail(errorMessage);
        }
        
        var sharedСhat =
            await _wrapper.ChatRepository.GetPersonalOrDefaultChatAsync(userId,
                request.SecondUserId);
        
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
                ProfileId = userId,
                Role = ChatRole.Admin,
                Status = ChatStatus.Ok,
            };

            var newUserOfChatSecond = new UserOfChat()
            {
                ChatId = createdPersonalChat.ChatId,
                ProfileId = request.SecondUserId,
                Role = ChatRole.Admin,
                Status = ChatStatus.Ok,
            };

            await _wrapper.UserOfChatRepository.CreateAsync(newUserOfChatFirst);
            await _wrapper.UserOfChatRepository.CreateAsync(newUserOfChatSecond);
            await _wrapper.SaveChangesAsync();

            var createdPersonalChatDto = _mapper.Map<PersonalChatDto>(createdPersonalChat);
            createdPersonalChatDto.SecondUserId = request.SecondUserId;

            return Result.Ok(createdPersonalChatDto);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
        
    }
}
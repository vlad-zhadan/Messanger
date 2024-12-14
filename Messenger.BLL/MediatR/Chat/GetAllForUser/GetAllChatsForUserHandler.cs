using AutoMapper;
using FluentResults;
using MediatR;
using Messenger.BLL.DTO.Chat;
using Messenger.BLL.DTO.GroupChat;
using Messenger.BLL.DTO.PersonalChat;
using Messenger.DAL.Enums;
using Messenger.DAL.Repositories.Interfaces.Base;
using Microsoft.EntityFrameworkCore;

namespace Messenger.BLL.MediatR.PersonalChat.GetAllForUser;

public class GetAllChatsForUserHandler : IRequestHandler<GetAllChatsForUserQuery, Result<IEnumerable<ChatDto>>>
{
    private readonly IRepositoryWrapper _wrapper;
    private readonly IMapper _mapper;

    public GetAllChatsForUserHandler(IRepositoryWrapper wrapper, IMapper mapper)
    {
        _wrapper = wrapper;
        _mapper = mapper;
    }
    
    public async Task<Result<IEnumerable<ChatDto>>> Handle(GetAllChatsForUserQuery request, CancellationToken cancellationToken)
    {
        var userOfChats = await _wrapper.UserOfChatRepository.GetAllAsync(
            predicate: cou => cou.ProfileId == request.UserId,
            include: source => source.Include(cou => cou.Chat),
            orderByDESC: uoc => uoc.Chat.LastMessage != null ? uoc.Chat.LastMessage.TimeSent : uoc.Chat.CreatedAt
        );

        try
        {
            List<ChatDto> userChats = new();
            foreach (var userOfChat in userOfChats)
            {
                if (userOfChat.Status is ChatStatus.Blocked or ChatStatus.Blocking)
                {
                    continue;
                }
                
                var numberOfUnreadMessages = await 
                    _wrapper.MessageRepository.GetNumberOfUnreadMessagesByUserOfChatAsync(userOfChat.UserOfChatId);
                    
                if (userOfChat.Chat.Type == ChatType.PersonalChat)
                {
                    var chatDto = _mapper.Map<PersonalChatDto>(userOfChat.Chat);
                    chatDto.SecondUserId = userOfChat.ProfileId;
                    chatDto.NumberOfUnreadMessages = numberOfUnreadMessages;
                    
                    userChats.Add(chatDto);
                    
                    // if (userOfChat.Chat.Type == ChatType.GroupChat)
                }else 
                {
                    var chatDto = _mapper.Map<GroupChatDto>(userOfChat.Chat);
                    chatDto.NumberOfUnreadMessages = numberOfUnreadMessages;
                    
                    userChats.Add(chatDto);
                }
                
            }
            
            return Result.Ok(userChats.AsEnumerable());

        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }
}
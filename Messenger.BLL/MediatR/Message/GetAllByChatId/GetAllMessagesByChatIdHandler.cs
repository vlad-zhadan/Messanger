using AutoMapper;
using FluentResults;
using MediatR;
using Mesagger.BLL.Security.Interface;
using Messenger.BLL.DTO.PersonalChatMessageDTO;
using Messenger.DAL.Repositories.Interfaces.Base;
using Microsoft.EntityFrameworkCore;

namespace Messenger.BLL.MediatR.PersonalMessage.GetAllByChatId;

public class GetAllMessagesByChatIdHandler : IRequestHandler<GetAllMessagesByChatIdQuery, Result<IEnumerable<MessageReceiveDto>>>
{
    private readonly IRepositoryWrapper _wrapper;
    private readonly IMapper _mapper;
    private readonly IUserAccessor _userAccessor;


    public GetAllMessagesByChatIdHandler(IRepositoryWrapper wrapper, IMapper mapper, IUserAccessor userAccessor)
    {
        _wrapper = wrapper;
        _mapper = mapper;
        _userAccessor = userAccessor;
    }
    public async Task<Result<IEnumerable<MessageReceiveDto>>> Handle(GetAllMessagesByChatIdQuery request, CancellationToken cancellationToken)
    {
        var userId = _userAccessor.GetCurrentUserId();

        if (userId < 0)
        {
            var errorMessage = $"User {userId} not found";
            return Result.Fail(errorMessage);
        }

        var userOfChatThatWantToGetMessages = await _wrapper.UserOfChatRepository.GetFirstOrDefaultAsync(
            predicate: uoc => uoc.ChatId == request.ChatId
                              && uoc.ProfileId == userId
        );

        if (userOfChatThatWantToGetMessages is null)
        {
            var errorMessage = $"User {userId} not found in the chat!";
            return Result.Fail(errorMessage);
        }
        
        var messages = await _wrapper.MessageRepository.GetAllAsync(
            predicate: m => m.MessageOwner.ChatId == request.ChatId, 
            include: source => source
                .Include(m => m.MessageOwner)
                .Include(m => m.Receivers)
            );

        try
        {
            var messagesDto = _mapper.Map<IEnumerable<MessageReceiveDto>>(messages);
            return Result.Ok(messagesDto);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
        
    }
}
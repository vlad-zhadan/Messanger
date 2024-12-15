using AutoMapper;
using FluentResults;
using MediatR;
using Mesagger.BLL.Security.Interface;
using Messenger.BLL.DTO.MessageReceiver;
using Messenger.BLL.DTO.PersonalChatMessageDTO;
using Messenger.DAL.Repositories.Interfaces.Base;
using Microsoft.EntityFrameworkCore;

namespace Messenger.BLL.MediatR.Message.GetById;

public class GetMessageByIdHandler : IRequestHandler<GetMessageByIdQuery, Result<MessageReceiveDto>>
{
    private readonly IRepositoryWrapper _wrapper;
    private readonly IMapper _mapper;
    private readonly IUserAccessor _userAccessor;

    public GetMessageByIdHandler(IRepositoryWrapper wrapper, IMapper mapper, IUserAccessor userAccessor)
    {
        _wrapper = wrapper;
        _mapper = mapper;
        _userAccessor = userAccessor;
    }
    public async Task<Result<MessageReceiveDto>> Handle(GetMessageByIdQuery request, CancellationToken cancellationToken)
    {
        var userId = _userAccessor.GetCurrentUserId();

        if (userId < 0)
        {
            var errorMessage = $"User {userId} not found";
            return Result.Fail(errorMessage);
        }
        
        var message = await _wrapper.MessageRepository.GetFirstOrDefaultAsync(
            predicate: m => m.MessageId == request.MessageId,
            include: source => source
                .Include(m => m.MessageOwner)
                .Include(m => m.Receivers)
        );

        if (message is null)
        {
            var errorMessage = $"Message with id: {request.MessageId} does not exist";
            return Result.Fail(errorMessage);
        }
        
        var userOfChatThatWantToGetMessages = await _wrapper.UserOfChatRepository.GetFirstOrDefaultAsync(
            predicate: uoc => uoc.ChatId == message.MessageOwner.ChatId
                              && uoc.ProfileId == userId
        );

        if (userOfChatThatWantToGetMessages is null)
        {
            var errorMessage = $"User {userId} not found in the chat!";
            return Result.Fail(errorMessage);
        }
        
        var messageDto = _mapper.Map<MessageReceiveDto>(message);
        
        return Result.Ok(messageDto);
    }
}
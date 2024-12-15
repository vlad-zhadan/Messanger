using AutoMapper;
using FluentResults;
using MediatR;
using Mesagger.BLL.Security.Interface;
using Messenger.BLL.DTO.LastSeen;
using Messenger.DAL.Enums;
using Messenger.DAL.Repositories.Interfaces.Base;

namespace Messenger.BLL.MediatR.LastSeen.Get;

public class GetProfileLastSeenHandler : IRequestHandler<GetProfileLastSeenQuery, Result<LastSeenDto>>
{
    private readonly IRepositoryWrapper _wrapper;
    private readonly IMapper _mapper;
    private readonly IUserAccessor _userAccessor;

    public GetProfileLastSeenHandler(IRepositoryWrapper wrapper, IMapper mapper, IUserAccessor userAccessor)
    {
        _wrapper = wrapper;
        _mapper = mapper;
        _userAccessor = userAccessor;
    }
    public async Task<Result<LastSeenDto>> Handle(GetProfileLastSeenQuery request, CancellationToken cancellationToken)
    {
        var userId = _userAccessor.GetCurrentUserId();

        if (userId < 0)
        {
            var errorMessage = $"User {userId} not found";
            return Result.Fail(errorMessage);
        }

        var personalChat = await _wrapper.ChatRepository.GetPersonalOrDefaultChatAsync(userId, request.PersonId);

        if (personalChat is not null)
        {
            var currentUserOfChat = await _wrapper.UserOfChatRepository.GetFirstOrDefaultAsync(
                predicate: uoc => uoc.ChatId == personalChat.ChatId
                                  && uoc.ProfileId == userId
            );

            if (currentUserOfChat?.Status is ChatStatus.Blocked or ChatStatus.Blocking)
            {
                var errorMessage = $"User {userId} has already been blocked and you dont have permission to get profile last seen";
                return Result.Fail(errorMessage);
            }
        }
        
        var profile =
            await _wrapper.ProfileRepository.GetFirstOrDefaultAsync(predicate: p => p.ProfileId == request.PersonId);

        if (profile is null)
        {
            var errorMessage = $"Profile with id {request.PersonId} does not exist.";
            return Result.Fail(errorMessage);
        }
        
        var lastSeenStatus = _mapper.Map<LastSeenDto>(profile);
        
        return Result.Ok(lastSeenStatus);
    }
}
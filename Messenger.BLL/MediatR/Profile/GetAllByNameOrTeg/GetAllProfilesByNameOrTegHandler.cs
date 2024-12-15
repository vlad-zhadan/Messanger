using AutoMapper;
using FluentResults;
using MediatR;
using Mesagger.BLL.Security.Interface;
using Messenger.BLL.DTO.Profile;
using Messenger.DAL.Enums;
using Messenger.DAL.Repositories.Interfaces.Base;
using Microsoft.EntityFrameworkCore;

namespace Messenger.BLL.MediatR.Profile;

public class GetAllProfilesByNameOrTegHandler : IRequestHandler<GetAllProfilesByNameOrTegQuery, Result<IEnumerable<ProfileDto>>>
{
    private readonly IRepositoryWrapper _wrapper;
    private readonly IMapper _mapper;
    private readonly IUserAccessor _userAccessor;

    public GetAllProfilesByNameOrTegHandler(IRepositoryWrapper wrapper, IMapper mapper, IUserAccessor userAccessor)
    {
        _wrapper = wrapper;
        _mapper = mapper;
        _userAccessor = userAccessor;
    }
    
    public async Task<Result<IEnumerable<ProfileDto>>> Handle(GetAllProfilesByNameOrTegQuery request, CancellationToken cancellationToken)
    {
        var userId = _userAccessor.GetCurrentUserId();

        if (userId < 0)
        {
            var errorMessage = $"User {userId} not found";
            return Result.Fail(errorMessage);
        }

        var blockedProfiles =
            await _wrapper.ProfileRepository.GetAllProfilesInChatTypeWithChatStatusAsync(userId, ChatType.PersonalChat,
                ChatStatus.Blocked);
        
        var blokingProfiles = await _wrapper.ProfileRepository.GetAllProfilesInChatTypeWithChatStatusAsync(userId, ChatType.PersonalChat,
            ChatStatus.Blocking);
        
        var profiles = await _wrapper.ProfileRepository.GetAllAsync(
            predicate:
            p => EF.Functions.Like(p.FirstName, $"%{request.NameOrTag.Trim()}%")
                 || EF.Functions.Like(p.LastName, $"%{request.NameOrTag.Trim()}%")
                 || EF.Functions.Like(p.Tag, $"%{request.NameOrTag.Trim()}%")
        );
        
        var filteredProfiles = profiles
            .Where(p => blockedProfiles.All(bp => bp.ProfileId != p.ProfileId) && blokingProfiles.All(bp => bp.ProfileId != p.ProfileId))
            .ToList();
        
        var profilesDto = _mapper.Map<IEnumerable<ProfileDto>>(filteredProfiles);
        return Result.Ok(profilesDto);
    }
}
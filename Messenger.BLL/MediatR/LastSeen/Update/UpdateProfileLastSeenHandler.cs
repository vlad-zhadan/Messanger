using FluentResults;
using MediatR;
using Mesagger.BLL.Security.Interface;
using Messenger.DAL.Repositories.Interfaces.Base;

namespace Messenger.BLL.MediatR.Profile.UpdateLastSeenOnConnect;

public class UpdateProfileLastSeenHandler : IRequestHandler<UpdateProfileLastSeenCommand, Result<int>>
{
    private readonly IRepositoryWrapper _wrapper;
    private readonly IUserAccessor _userAccessor;

    public UpdateProfileLastSeenHandler(IRepositoryWrapper wrapper, IUserAccessor userAccessor)
    {
        _wrapper = wrapper;
        _userAccessor = userAccessor;
    }
    
    public async Task<Result<int>> Handle(UpdateProfileLastSeenCommand request, CancellationToken cancellationToken)
    {
        var userId = _userAccessor.GetCurrentUserId();

        if (userId < 0)
        {
            var errorMessage = $"User {userId} not found";
            return Result.Fail(errorMessage);
        }
        
        var profile = await 
            _wrapper.ProfileRepository.GetFirstOrDefaultAsync(predicate: p =>
                p.ProfileId == userId);

        if (profile is null)
        {
            var errorMessage = $"Profile with ID {userId} does not exist.";
            return Result.Fail(errorMessage);
        }

        if (request.LastSeen.IsOnline)
        {
            profile.LastSeen = default;
        }
        else
        {
            profile.LastSeen = request.LastSeen.LastSeen;
        }
        
        _wrapper.ProfileRepository.Update(profile);
        await _wrapper.SaveChangesAsync();

        return Result.Ok(profile.ProfileId);
    }
}
using FluentResults;
using MediatR;
using Messenger.DAL.Repositories.Interfaces.Base;

namespace Messenger.BLL.MediatR.Profile.UpdateLastSeenOnConnect;

public class UpdateProfileLastSeenHandler : IRequestHandler<UpdateProfileLastSeenCommand, Result<int>>
{
    private readonly IRepositoryWrapper _wrapper;

    public UpdateProfileLastSeenHandler(IRepositoryWrapper wrapper)
    {
        _wrapper = wrapper;
    }
    
    public async Task<Result<int>> Handle(UpdateProfileLastSeenCommand request, CancellationToken cancellationToken)
    {
        var profile = await 
            _wrapper.ProfileRepository.GetFirstOrDefaultAsync(predicate: p =>
                p.ProfileId == request.LastSeen.ProfileId);

        if (profile is null)
        {
            var errorMessage = $"Profile with ID {request.LastSeen.ProfileId} does not exist.";
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
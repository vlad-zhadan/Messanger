using FluentResults;
using MediatR;
using Messenger.DAL.Repositories.Interfaces.Base;

namespace Mesagger.BLL.MediatR.Profile.UpdateConnection;

public class UpdateProfileConnectionHandler : IRequestHandler<UpdateProfileConnectionCommand, Result<int>>
{
    private readonly IRepositoryWrapper _wrapper;

    public UpdateProfileConnectionHandler(IRepositoryWrapper wrapper)
    {
        _wrapper = wrapper;
    }
    public async Task<Result<int>> Handle(UpdateProfileConnectionCommand request, CancellationToken cancellationToken)
    {
        var profile =
            await _wrapper.ProfileRepository.GetFirstOrDefaultAsync(predicate: p =>
                p.ProfileId == request.ConnectionDto.ProfileId);

        if (profile is null)
        {
            var errorMessage = $"Profile with id {request.ConnectionDto.ProfileId} was not found";
            return Result.Fail(errorMessage);
        }
        
        profile.ConnectionId = request.ConnectionDto.ConnectionId;
        var updatedConnection = _wrapper.ProfileRepository.Update(profile);
        await _wrapper.SaveChangesAsync();
        
        return Result.Ok(updatedConnection.Entity.ProfileId);
    }
}
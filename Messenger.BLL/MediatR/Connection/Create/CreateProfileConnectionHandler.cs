using FluentResults;
using MediatR;
using Messenger.DAL.Entities;
using Messenger.DAL.Repositories.Interfaces.Base;

namespace Mesagger.BLL.MediatR.Profile.UpdateConnection;

public class CreateProfileConnectionHandler : IRequestHandler<CreateProfileConnectionCommand, Result<int>>
{
    private readonly IRepositoryWrapper _wrapper;

    public CreateProfileConnectionHandler(IRepositoryWrapper wrapper)
    {
        _wrapper = wrapper;
    }
    public async Task<Result<int>> Handle(CreateProfileConnectionCommand request, CancellationToken cancellationToken)
    {
        var profile =
            await _wrapper.ProfileRepository.GetFirstOrDefaultAsync(predicate: p =>
                p.ProfileId == request.ConnectionDto.ProfileId);

        if (profile is null)
        {
            var errorMessage = $"Profile with id {request.ConnectionDto.ProfileId} was not found";
            return Result.Fail(errorMessage);
        }

        var connection = new Connection()
        {
            ProfileId = profile.ProfileId,
            ConnectionString = request.ConnectionDto.ConnectionId
        };
        var createdConnection = await _wrapper.ConnectionRepository.CreateAsync(connection);
        await _wrapper.SaveChangesAsync();
        
        return Result.Ok(createdConnection.ProfileId);
    }
}
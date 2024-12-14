using FluentResults;
using MediatR;
using Messenger.DAL.Repositories.Interfaces.Base;
using Microsoft.EntityFrameworkCore;

namespace Mesagger.BLL.MediatR.Connection.Get;

public class GetConectionsOfUserHandler : IRequestHandler<GetConectionsOfUserQuery, Result<IEnumerable<string>>>
{
    private readonly IRepositoryWrapper _wrapper;

    public GetConectionsOfUserHandler(IRepositoryWrapper wrapper)
    {
        _wrapper = wrapper;
    }
    
    public async Task<Result<IEnumerable<string>>> Handle(GetConectionsOfUserQuery request, CancellationToken cancellationToken)
    {
        var profile = await _wrapper.ProfileRepository.GetFirstOrDefaultAsync(
            predicate: p => p.ProfileId == request.UserId,
            include: src => src.Include(p => p.Connections)
        );

        if (profile is null)
        {
            var errorMessage = $"Profile with id {request.UserId} was not found";
            return Result.Fail(errorMessage);
        }
        
        return Result.Ok(profile.Connections.Select(c => c.ConnectionString));
    }
}
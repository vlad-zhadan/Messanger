using FluentResults;
using MediatR;
using Messenger.DAL.Repositories.Interfaces.Base;

namespace Mesagger.BLL.MediatR.Connection.GetPersonByConnection;

public class GetPersonIdByConnectionHandler : IRequestHandler<GetPersonIdByConnectionQuery, Result<int>>
{
    private readonly IRepositoryWrapper _wrapper;

    public GetPersonIdByConnectionHandler(IRepositoryWrapper wrapper)
    {
        _wrapper = wrapper;
    }
    
    public async Task<Result<int>> Handle(GetPersonIdByConnectionQuery request, CancellationToken cancellationToken)
    {
        var connection = await _wrapper.ConnectionRepository.GetFirstOrDefaultAsync(predicate: c=> c.ConnectionString == request.ConnectionString);

        if (connection is null)
        {
            var errorMessage = $"Connection {request.ConnectionString} was not found";
            return Result.Fail(errorMessage);
        }

        return Result.Ok(connection.ProfileId);
    }
}
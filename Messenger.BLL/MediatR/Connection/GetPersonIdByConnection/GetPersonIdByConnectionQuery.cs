using FluentResults;
using MediatR;

namespace Mesagger.BLL.MediatR.Connection.GetPersonByConnection;

public record GetPersonIdByConnectionQuery(string ConnectionString) : IRequest<Result<int>>;
using FluentResults;
using MediatR;

namespace Mesagger.BLL.MediatR.Connection.GetPersonByConnection;

public record GetPersonIdByConnectionCommand(string ConnectionString) : IRequest<Result<int>>;
using FluentResults;
using MediatR;

namespace Messenger.BLL.MediatR.Connection.GetPersonByConnection;

public record GetPersonIdByConnectionCommand(string ConnectionString) : IRequest<Result<int>>;
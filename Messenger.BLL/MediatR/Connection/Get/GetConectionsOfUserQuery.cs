using FluentResults;
using MediatR;

namespace Mesagger.BLL.MediatR.Connection.Get;

public record GetConectionsOfUserQuery(int UserId) : IRequest<Result<IEnumerable<string>>>;
using FluentResults;
using MediatR;

namespace Mesagger.BLL.MediatR.Profile;

public record CheckProfileTagQuery(string Tag) : IRequest<Result<bool>>;
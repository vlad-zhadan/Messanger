using FluentResults;
using MediatR;

namespace Mesagger.BLL.MediatR.Message.Delete;

public record DeleteMessageQuery(int MessageId) : IRequest<Result<int>>;
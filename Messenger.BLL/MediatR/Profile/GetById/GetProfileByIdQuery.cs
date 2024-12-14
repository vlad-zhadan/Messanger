using FluentResults;
using MediatR;
using Messenger.BLL.DTO.Profile;

namespace Messenger.BLL.MediatR.Profile.GetById;

public record GetProfileByIdQuery(int ProfileId) : IRequest<Result<ProfileDto>>;
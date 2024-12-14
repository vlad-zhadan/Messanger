using FluentResults;
using MediatR;
using Messenger.BLL.DTO.Profile;

namespace Mesagger.BLL.MediatR.Profile.GetByUserId;

public record GetProfileByUserIdQuery(int UserId) : IRequest<Result<ProfileDto>>;
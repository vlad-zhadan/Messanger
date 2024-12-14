using FluentResults;
using MediatR;
using Messenger.BLL.DTO.Profile;

namespace Messenger.BLL.MediatR.Profile.Create;

public record CreateProfileCommand(ProfileCreateDto NewProfile) : IRequest<Result<ProfileDto>>;
using FluentResults;
using MediatR;
using Messenger.BLL.DTO.PersonalChat;
using Messenger.BLL.DTO.Profile;

namespace Messenger.BLL.MediatR.Profile.Update;

public record UpdateProfileCommand(ProfileUpdateDto UpdatedProfile) : IRequest<Result<ProfileDto>>;
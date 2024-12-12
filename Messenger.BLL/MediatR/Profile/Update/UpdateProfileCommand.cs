using FluentResults;
using MediatR;
using Mesagger.BLL.DTO.PersonalChat;
using Mesagger.BLL.DTO.Profile;

namespace Mesagger.BLL.MediatR.Profile.Update;

public record UpdateProfileCommand(ProfileUpdateDto UpdatedProfile) : IRequest<Result<ProfileDto>>;
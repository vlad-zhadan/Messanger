using FluentResults;
using MediatR;
using Mesagger.BLL.DTO.Profile;

namespace Mesagger.BLL.MediatR.Profile.Create;

public record CreateProfileCommand(ProfileCreateDto NewProfile) : IRequest<Result<ProfileDto>>;
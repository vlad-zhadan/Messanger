using FluentResults;
using MediatR;
using Mesagger.BLL.DTO.Profile;

namespace Mesagger.BLL.MediatR.Profile.UpdateConnection;

public record UpdateProfileConnectionCommand(ProfileUpdateConnectionDto ConnectionDto) : IRequest<Result<int>>;
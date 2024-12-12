using FluentResults;
using MediatR;
using Mesagger.BLL.DTO.Profile;

namespace Mesagger.BLL.MediatR.Profile.UpdateConnection;

public record CreateProfileConnectionCommand(ProfileUpdateConnectionDto ConnectionDto) : IRequest<Result<int>>;
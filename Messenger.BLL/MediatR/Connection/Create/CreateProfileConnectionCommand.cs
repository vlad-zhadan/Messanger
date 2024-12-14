using FluentResults;
using MediatR;
using Messenger.BLL.DTO.Profile;

namespace Messenger.BLL.MediatR.Profile.UpdateConnection;

public record CreateProfileConnectionCommand(ProfileUpdateConnectionDto ConnectionDto) : IRequest<Result<int>>;
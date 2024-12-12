using AutoMapper;
using FluentResults;
using MediatR;
using Mesagger.BLL.DTO.LastSeen;
using Messenger.DAL.Repositories.Interfaces.Base;

namespace Mesagger.BLL.MediatR.LastSeen.Get;

public class GetProfileLastSeenHandler : IRequestHandler<GetProfileLastSeenQuery, Result<LastSeenDto>>
{
    private readonly IRepositoryWrapper _wrapper;
    private readonly IMapper _mapper;

    public GetProfileLastSeenHandler(IRepositoryWrapper wrapper, IMapper mapper)
    {
        _wrapper = wrapper;
        _mapper = mapper;
    }
    public async Task<Result<LastSeenDto>> Handle(GetProfileLastSeenQuery request, CancellationToken cancellationToken)
    {
        var profile =
            await _wrapper.ProfileRepository.GetFirstOrDefaultAsync(predicate: p => p.ProfileId == request.PersonId);

        if (profile is null)
        {
            var errorMessage = $"Profile with id {request.PersonId} does not exist.";
            return Result.Fail(errorMessage);
        }
        
        var lastSeenStatus = _mapper.Map<LastSeenDto>(profile);
        
        return Result.Ok(lastSeenStatus);
    }
}
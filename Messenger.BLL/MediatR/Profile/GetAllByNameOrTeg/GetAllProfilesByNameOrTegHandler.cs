using AutoMapper;
using FluentResults;
using MediatR;
using Messenger.BLL.DTO.Profile;
using Messenger.DAL.Repositories.Interfaces.Base;
using Microsoft.EntityFrameworkCore;

namespace Messenger.BLL.MediatR.Profile;

public class GetAllProfilesByNameOrTegHandler : IRequestHandler<GetAllProfilesByNameOrTegQuery, Result<IEnumerable<ProfileDto>>>
{
    private readonly IRepositoryWrapper _wrapper;
    private readonly IMapper _mapper;

    public GetAllProfilesByNameOrTegHandler(IRepositoryWrapper wrapper, IMapper mapper)
    {
        _wrapper = wrapper;
        _mapper = mapper;
    }
    
    public async Task<Result<IEnumerable<ProfileDto>>> Handle(GetAllProfilesByNameOrTegQuery request, CancellationToken cancellationToken)
    {
        var profiles = await _wrapper.ProfileRepository.GetAllAsync(
            predicate:
            p => EF.Functions.Like(p.FirstName, $"%{request.NameOrTag.Trim()}%")
                 || EF.Functions.Like(p.LastName, $"%{request.NameOrTag.Trim()}%")
                 || EF.Functions.Like(p.Tag, $"%{request.NameOrTag.Trim()}%")
        );
        
        var profilesDto = _mapper.Map<IEnumerable<ProfileDto>>(profiles);
        return Result.Ok(profilesDto);
    }
}
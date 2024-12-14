using AutoMapper;
using FluentResults;
using MediatR;
using Messenger.BLL.DTO.Profile;
using Messenger.DAL.Repositories.Interfaces.Base;

namespace Messenger.BLL.MediatR.Profile.GetById;

public class GetProfileByIdHandler : IRequestHandler<GetProfileByIdQuery, Result<ProfileDto>>
{
    private readonly IRepositoryWrapper _wrapper;
    private readonly IMapper _mapper;

    public GetProfileByIdHandler(IRepositoryWrapper wrapper, IMapper mapper)
    {
        _wrapper = wrapper;
        _mapper = mapper;
    }
    
    public async Task<Result<ProfileDto>> Handle(GetProfileByIdQuery request, CancellationToken cancellationToken)
    {
        var profile = await _wrapper.ProfileRepository.GetFirstOrDefaultAsync(predicate: p=> p.ProfileId == request.ProfileId);

        if (profile is null)
        {
            var error = new Error($"Profile with id: {request.ProfileId} does not exist");
            return Result.Fail(error);
        }

        try
        {
            var profileDto = _mapper.Map<ProfileDto>(profile);
            return Result.Ok(profileDto);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }
}
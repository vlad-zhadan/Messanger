using AutoMapper;
using FluentResults;
using MediatR;
using Messenger.BLL.DTO.Profile;
using Messenger.DAL.Repositories.Interfaces.Base;

namespace Mesagger.BLL.MediatR.Profile.GetByUserId;

public class GetProfileByUserIdHandler : IRequestHandler<GetProfileByUserIdQuery, Result<ProfileDto>>
{
    private readonly IRepositoryWrapper _wrapper;
    private readonly IMapper _mapper;

    public GetProfileByUserIdHandler(IRepositoryWrapper wrapper, IMapper mapper)
    {
        _wrapper = wrapper;
        _mapper = mapper;
    }
    public async Task<Result<ProfileDto>> Handle(GetProfileByUserIdQuery request, CancellationToken cancellationToken)
    {
        var profile = await _wrapper.ProfileRepository.GetFirstOrDefaultAsync(
            predicate: p => p.UserId == request.UserId);

        if (profile is null)
        {
            var errorMessage = $"User {request.UserId} does not exist.";
            return Result.Fail(errorMessage);
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
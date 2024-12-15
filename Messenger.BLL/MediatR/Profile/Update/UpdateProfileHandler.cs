using AutoMapper;
using FluentResults;
using MediatR;
using Mesagger.BLL.Security.Interface;
using Messenger.BLL.DTO.Profile;
using Messenger.DAL.Repositories.Interfaces.Base;

namespace Messenger.BLL.MediatR.Profile.Update;

public class UpdateProfileHandler : IRequestHandler<UpdateProfileCommand, Result<ProfileDto>>
{
    private readonly IRepositoryWrapper _wrapper;
    private readonly IMapper _mapper;
    private readonly IUserAccessor _userAccessor;

    public UpdateProfileHandler(IRepositoryWrapper wrapper, IMapper mapper, IUserAccessor userAccessor)
    {
        _wrapper = wrapper;
        _mapper = mapper;
        _userAccessor = userAccessor;
    }
    public async Task<Result<ProfileDto>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var userId = _userAccessor.GetCurrentUserId();

        if (userId < 0)
        {
            var errorMessage = $"User {userId} not found";
            return Result.Fail(errorMessage);
        }
        
        var existedPofileToChange = await
            _wrapper.ProfileRepository.GetFirstOrDefaultAsync(predicate: p =>
                p.ProfileId == userId);

        if (existedPofileToChange is null)
        {
            var errorMessage = $"Profile with id: {userId} was not found";
            return Result.Fail(errorMessage);
        }

        try
        {
            var profileToChange = _mapper.Map<Messenger.DAL.Entities.Profile>(request.UpdatedProfile);
            var updatedProfile = _wrapper.ProfileRepository.Update(profileToChange);
            await _wrapper.SaveChangesAsync();
            
            var updatedProfileDto = _mapper.Map<ProfileDto>(updatedProfile);
            return Result.Ok(updatedProfileDto);

        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }
}
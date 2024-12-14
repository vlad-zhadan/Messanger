using AutoMapper;
using FluentResults;
using MediatR;
using Messenger.BLL.DTO.Profile;
using Messenger.DAL.Repositories.Interfaces.Base;

namespace Messenger.BLL.MediatR.Profile.Update;

public class UpdateProfileHandler : IRequestHandler<UpdateProfileCommand, Result<ProfileDto>>
{
    private readonly IRepositoryWrapper _wrapper;
    private readonly IMapper _mapper;

    public UpdateProfileHandler(IRepositoryWrapper wrapper, IMapper mapper)
    {
        _wrapper = wrapper;
        _mapper = mapper;
    }
    public async Task<Result<ProfileDto>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var existedPofileToChange = await
            _wrapper.ProfileRepository.GetFirstOrDefaultAsync(predicate: p =>
                p.ProfileId == request.UpdatedProfile.ProfileId);

        if (existedPofileToChange is null)
        {
            var errorMessage = $"Profile with id: {request.UpdatedProfile.ProfileId} was not found";
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
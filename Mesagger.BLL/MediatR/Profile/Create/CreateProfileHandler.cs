using AutoMapper;
using FluentResults;
using MediatR;
using Mesagger.BLL.DTO.Profile;
using Messenger.DAL.Repositories.Interfaces.Base;

namespace Mesagger.BLL.MediatR.Profile.Create;

public class CreateProfileHandler : IRequestHandler<CreateProfileCommand, Result<ProfileDto>>
{
    private readonly IRepositoryWrapper _wrapper;
    private readonly IMapper _mapper;

    public CreateProfileHandler(IRepositoryWrapper wrapper, IMapper mapper)
    {
        _wrapper = wrapper;
        _mapper = mapper;
    }
    
    public async Task<Result<ProfileDto>> Handle(CreateProfileCommand request, CancellationToken cancellationToken)
    {
        var profileWithSameTag = await _wrapper.ProfileRepository.GetFirstOrDefaultAsync(p => p.Tag == request.NewProfile.Tag);

        if (profileWithSameTag is not null)
        {
            var errorMessage = $"Profile with tag {request.NewProfile.Tag} does exist already.";
            return Result.Fail(errorMessage);
        }

        try
        {
            var newProfile = _mapper.Map<Messenger.DAL.Entities.Profile>(request.NewProfile);
            var createdProfile = await _wrapper.ProfileRepository.CreateAsync(newProfile);
            await _wrapper.SaveChangesAsync();
            
            var createdProfileDto = _mapper.Map<ProfileDto>(createdProfile);
            return Result.Ok(createdProfileDto);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }
}
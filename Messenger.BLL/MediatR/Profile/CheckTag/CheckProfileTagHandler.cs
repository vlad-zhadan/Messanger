using FluentResults;
using MediatR;
using Messenger.DAL.Repositories.Interfaces.Base;

namespace Mesagger.BLL.MediatR.Profile;

public class CheckProfileTagHandler : IRequestHandler<CheckProfileTagQuery, Result<bool>>
{
    private readonly IRepositoryWrapper _wrapper;

    public CheckProfileTagHandler(IRepositoryWrapper wrapper)
    {
        _wrapper = wrapper;
    }
    public async Task<Result<bool>> Handle(CheckProfileTagQuery request, CancellationToken cancellationToken)
    {
        var profileWithSameTag = await _wrapper.ProfileRepository.GetFirstOrDefaultAsync(p => p.Tag == request.Tag);

        if (profileWithSameTag is not null)
        {
            return Result.Ok(false);
        }
        
        return Result.Ok(true);
    }
}
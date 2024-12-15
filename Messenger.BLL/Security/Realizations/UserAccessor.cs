using System.Security.Claims;
using Mesagger.BLL.Security.Interface;
using Microsoft.AspNetCore.Http;

namespace Mesagger.BLL.Security.Realizations;

public class UserAccessor : IUserAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public int GetCurrentUserId()
    {
        return int.TryParse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier),
            out int userId)
            ? userId
            : -1;
    }
}
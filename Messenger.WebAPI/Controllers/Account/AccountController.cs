using System.Security.Claims;
using MediatR;
using Mesagger.BLL.DTO.User;
using Mesagger.BLL.MediatR.Profile;
using Mesagger.BLL.MediatR.Profile.GetByUserId;
using Mesagger.BLL.Services.Realizations;
using Messenger.BLL.DTO.Profile;
using Messenger.BLL.MediatR.Profile.Create;
using Messenger.BLL.MediatR.Profile.GetById;
using Messenger.DAL.Entities;
using Messenger.WebAPI.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Messenger.WebAPI.Controllers.Account;

public class AccountController : BaseController
{
    private readonly UserManager<User> _userManager;
    private readonly TokenService _tokenService;

    public AccountController(IMediator mediator, UserManager<User> userManager, TokenService tokenService) : base(mediator)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(UserLoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        
        if(user is null) return Unauthorized();
        
        var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
        
        if(!result) return Unauthorized();

        var profile = await _mediator.Send(new GetProfileByUserIdQuery(user.Id));
        
        if(profile.IsFailed) return Unauthorized();

        return new UserDto()
        {
            ProfileId = profile.Value.ProfileId,
            Token = _tokenService.CreateToken(user.Id),
        };
    }
    
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(UserRegisterDto registerDto)
    {
        if (await _userManager.Users.AnyAsync(x => x.Email == registerDto.User.Email))
        {
            ModelState.AddModelError("email", "Email taken");
            return ValidationProblem();
        }

        var isTagUnique = await _mediator.Send(new CheckProfileTagQuery(registerDto.Profile.Tag));
        if (!isTagUnique.Value)
        {
            ModelState.AddModelError("tag", "Tag taken");
            return ValidationProblem();
        }
        
        var user = new User
        {
            Email = registerDto.User.Email,
        };

        var result = await _userManager.CreateAsync(user, registerDto.User.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        var profile = await _mediator.Send(new CreateProfileCommand(registerDto.Profile, user.Id));

        if (profile.IsFailed)
        {
            return BadRequest(profile.Errors);
        }

        return Ok(profile.Value);
    }
    
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<ProfileDto>> GetCurrentUser()
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(x => x.Id.ToString() == User
                .FindFirstValue(ClaimTypes.NameIdentifier)
            );

        if (user is null) return Unauthorized();

        var profile = await _mediator.Send(new GetProfileByUserIdQuery(user.Id));
        
        if(profile.IsFailed) return BadRequest(profile.Errors);
        
        return Ok(profile.Value);
    }

}
using Messenger.BLL.DTO.Profile;

namespace Mesagger.BLL.DTO.User;

public class UserRegisterDto
{
    public ProfileCreateDto Profile { get; set; }
    public UserLoginDto User { get; set; }
}
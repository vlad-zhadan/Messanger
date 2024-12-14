using System.ComponentModel.DataAnnotations;

namespace Mesagger.BLL.DTO.User;

public class UserLoginDto
{
    [EmailAddress]
    [Required(AllowEmptyStrings = false)]
    [StringLength(300, ErrorMessage = "Max email name length is 300")]
    public string Email { get; set; }
    
    [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{4,8}$", ErrorMessage = "Password must be complex")]
    [Required]
    public string Password { get; set; }
}
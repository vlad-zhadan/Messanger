using System.ComponentModel.DataAnnotations;

namespace Mesagger.BLL.DTO.Profile;

public class ProfileCreateDto
{
    [Required(AllowEmptyStrings = false)]
    [StringLength(50, ErrorMessage = "Max first name length is 50")]
    public string FirstName { get; set; }
    
    [StringLength(50, ErrorMessage = "Max last name length is 50")]
    public string? LastName { get; set; }
    
    [Required(AllowEmptyStrings = false)]
    [StringLength(50, ErrorMessage = "Max tag length is 50")]
    public string Tag { get; set; }
    
    [StringLength(200, ErrorMessage = "Max bio length is 50")]
    public string? Bio { get; set; }
}
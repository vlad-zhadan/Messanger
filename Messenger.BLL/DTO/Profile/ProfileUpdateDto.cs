using System.ComponentModel.DataAnnotations;
using Messenger.BLL.Attributes.General;
using Messenger.BLL.Constants.General;

namespace Messenger.BLL.DTO.Profile;

public class ProfileUpdateDto
{
    [Required]
    [GreaterOrEqualThan(GeneralConstants.MinValueForId,ErrorMessage = "Id is invalid")]
    public int ProfileId { get; set; }
    
    [Required(AllowEmptyStrings = false)]
    [StringLength(50, ErrorMessage = "Max first name length is 50")]
    public string FirstName { get; set; }
    
    [StringLength(50, ErrorMessage = "Max last name length is 50")]
    public string? LastName { get; set; }
    
    [Required(AllowEmptyStrings = false)]
    [StringLength(50, ErrorMessage = "Max tag length is 50")]
    [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "Tag can only contain letters and numbers.")]
    public string Tag { get; set; }
    
    [StringLength(200, ErrorMessage = "Max bio length is 50")]
    public string? Bio { get; set; }
}
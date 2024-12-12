using System.ComponentModel.DataAnnotations;
using Mesagger.BLL.Attributes.General;
using Mesagger.BLL.Constants.General;

namespace Mesagger.BLL.DTO.PersonalChatMessageDTO;

public class PersonalMessageSendDto
{
    [Required]
    [GreaterOrEqualThan(GeneralConstants.MinValueForId,ErrorMessage = "Id is invalid")]
    public int UserOwnerId { get; set; }
    
    [Required(AllowEmptyStrings = false)]
    [StringLength(400, ErrorMessage = "Max text length is 400")]
    public string Text { get; set; }
    
    [Required]
    [GreaterOrEqualThan(GeneralConstants.MinValueForId,ErrorMessage = "Id is invalid")]
    public int ChatId { get; set; }
}
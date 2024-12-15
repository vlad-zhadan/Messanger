using System.ComponentModel.DataAnnotations;
using Messenger.BLL.Attributes.General;
using Messenger.BLL.Constants.General;

namespace Messenger.BLL.DTO.PersonalChatMessageDTO;

public class MessageSendDto
{
    [Required(AllowEmptyStrings = false)]
    [StringLength(400, ErrorMessage = "Max text length is 400")]
    public string Text { get; set; }
    
    [Required]
    [GreaterOrEqualThan(GeneralConstants.MinValueForId,ErrorMessage = "Id is invalid")]
    public int ChatId { get; set; }
}
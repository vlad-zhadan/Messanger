using System.ComponentModel.DataAnnotations;
using Messenger.BLL.Attributes.General;
using Messenger.BLL.Constants.General;

namespace Messenger.BLL.DTO.MessageReceiver;

public class MessageReceiverSendDto
{
    [Required]
    [GreaterOrEqualThan(GeneralConstants.MinValueForId,ErrorMessage = "Id is invalid")]
    public int MessageId { get; set; }
    
    [Required]
    [GreaterOrEqualThan(GeneralConstants.MinValueForId,ErrorMessage = "Id is invalid")]
    public int ChatId { get; set; }
}
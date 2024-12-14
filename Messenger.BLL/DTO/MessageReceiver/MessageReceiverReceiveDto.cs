namespace Messenger.BLL.DTO.MessageReceiver;

public class MessageReceiverReceiveDto
{
    //public int MessageReceiverId { get; set; }
    
    //public int ChatId { get; set; }
    
    public int MessageId { get; set; }

    public int ProfileReceiverId { get; set; }
    
    public DateTime TimeRead { get; set; }
}
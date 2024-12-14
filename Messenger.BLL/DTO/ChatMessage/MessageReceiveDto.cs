namespace Messenger.BLL.DTO.PersonalChatMessageDTO;

public class MessageReceiveDto
{
    public int MessageId { get; set; }
    public int ChatId { get; set; }
    public string Text { get; set; }
    public int UserOwnerId { get; set; }
    public DateTime TimeSent { get; set; }
    public IEnumerable<int> ReceiverIds { get; set; }
} 
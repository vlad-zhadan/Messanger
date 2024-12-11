namespace Mesagger.BLL.DTO.PersonalChatMessageDTO;

public class PersonalMessageReceiveDto
{
    public int MessageId { get; set; }
    public int ChatId { get; set; }
    public string Text { get; set; }
    public int UserMessageOwnerId { get; set; }
    public DateTime TimeSent { get; set; }
}
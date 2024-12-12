namespace Mesagger.BLL.DTO.PersonalChatMessageDTO;

public class PersonalMessageDto
{
    public int MessageId { get; set; }
    public string Text { get; set; }
    public int UserOwnerId { get; set; }
    public DateTime TimeSent { get; set; }
    public DateTime? TimeRead { get; set; }
}
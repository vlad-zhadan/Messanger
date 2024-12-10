namespace Mesagger.BLL.DTO.PersonalChatMessageDTO;

public class SendPersonalMessageDto
{
    public int UserOwnerId { get; set; }
    public string Text { get; set; }
    public int ChatId { get; set; }
}
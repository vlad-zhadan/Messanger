namespace Mesagger.BLL.DTO.PersonalChat;

public class PersonalChatBlockDto
{
    public int ChatId { get; set; }
    public int UserWhoBlockingId { get; set; }
    public int UserWhoBlockedId { get; set; }
}
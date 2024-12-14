using Messenger.DAL.Enums;

namespace Messenger.BLL.DTO.Chat;

public class ChatDto                                                                     
{
    public int ChatId { get; set; }                                                                                                           
    public ChatType Type { get; set; }
    public int? LastMessageId { get; set; }
    public int NumberOfUnreadMessages { get; set; }
}
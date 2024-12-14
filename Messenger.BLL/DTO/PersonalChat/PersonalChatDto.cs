using Messenger.BLL.DTO.Chat;
using Messenger.DAL.Enums;

namespace Messenger.BLL.DTO.PersonalChat;

public class PersonalChatDto : ChatDto
{
    public int SecondUserId { get; set; }
}
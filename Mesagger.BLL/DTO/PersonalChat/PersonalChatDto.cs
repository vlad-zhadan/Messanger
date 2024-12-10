using Messenger.DAL.Enums;
using Microsoft.VisualBasic.CompilerServices;

namespace Mesagger.BLL.DTO.PersonalChat;

public class PersonalChatDto
{
    public int ChatId { get; set; }
    public ChatType Type { get; set; }
    public ChatStatus Status { get; set; }
    public int SecondUserId { get; set; }
}
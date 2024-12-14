using Messenger.BLL.DTO.Chat;
using Messenger.DAL.Enums;

namespace Messenger.BLL.DTO.GroupChat;

public class GroupChatDto : ChatDto
{
    public string? Description { get; set; }
    public string? PictureId { get; set; }
}
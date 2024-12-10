using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Messenger.DAL.Enums;

namespace Messenger.DAL.Entities;

public class UserOfChat
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserOfChatId { get; set; }
    
    public int ChatId { get; set; }
    public Chat Chat { get; set; }
    
    public int ProfileId { get; set; }
    public Profile Profile { get; set; }
    
    [Required]
    public ChatRole Role { get; set; }
    public ChatStatus? Status { get; set; }
    
    public IEnumerable<Message> Messages { get; set; }
    public IEnumerable<MessageReceiver> MessageReceivers { get; set; }
}
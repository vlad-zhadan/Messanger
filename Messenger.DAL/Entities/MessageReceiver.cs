using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Messenger.DAL.Entities;

public class MessageReceiver
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int MessageReceiverId { get; set; }
    
    public int MessageID { get; set; }
    public Message Message { get; set; }
    
    public int UserReceiverId { get; set; }
    public UserOfChat UserReceiver { get; set; }
    
    [Required]
    public DateTime TimeRead { get; set; }
}
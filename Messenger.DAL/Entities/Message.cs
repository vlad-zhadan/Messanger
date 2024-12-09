using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Messenger.DAL.Enums;

public class Message
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int MessageID { get; set; }
    
    public int MessageOwnerID { get; set; }
    public UserOfChat MessageOwner { get; set; }
    
    // public int Forvarded { get; set; }
    
    [Required]
    public DateTime TimeSended { get; set; }
    
    [Required]
    public MessageStatus Status { get; set; }
    
    public DateTime TimeStatusChanged { get; set; }
    
    public int? ContentId { get; set; }
    
    [MaxLength(200)]
    public string? Text { get; set; }
    
    public IEnumerable<MessageReceiver> Receivers { get; set; }
}
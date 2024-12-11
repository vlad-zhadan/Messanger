using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Messenger.DAL.Enums;

namespace Messenger.DAL.Entities;

public class Message
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int MessageId { get; set; }
    
    public int MessageOwnerId { get; set; }
    public UserOfChat MessageOwner { get; set; }
    
    // public int Forvarded { get; set; }
    
    [Required]
    public DateTime TimeSent { get; set; }
    
    [Required]
    public MessageStatus Status { get; set; }
    
    public DateTime TimeStatusChanged { get; set; }
    
    public int? ContentId { get; set; }
    
    [MaxLength(400)]
    public string? Text { get; set; }
    
    public IEnumerable<MessageReceiver> Receivers { get; set; }
}
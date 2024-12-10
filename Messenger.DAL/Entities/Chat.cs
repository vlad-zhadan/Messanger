using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Messenger.DAL.Enums;

public class Chat
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ChatId { get; set; }
    
    [MaxLength(50)]
    public string Name { get; set; }
    
    [Required]
    public ChatType Type { get; set; }
    
    [MaxLength(100)]
    public string Description { get; set; }
    
    [Required]
    public int MaxParticipants { get; set; }
    
    [Required]
    public bool IsPrivate { get; set; }
    
    public DateTime DeletedAt { get; set; }
    
    public IEnumerable<UserOfChat> UsersOfChat { get; set; }
}
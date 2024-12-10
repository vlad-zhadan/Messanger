using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Messenger.DAL.Entities;

public class Profile
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ProfileId { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }
    
    [MaxLength(50)]
    public string? LastName { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Tag { get; set; }
    
    [MaxLength(200)]
    public string Bio { get; set; }
    
    [MaxLength(100)]
    public string PhotoBlobId { get; set; }
    
    public DateTime LastSeen { get; set; }
    
    public IEnumerable<UserContact> PersonalUserContacts  { get; set; }
    public IEnumerable<UserContact> ContactUserContacts { get; set; }
    
    public IEnumerable<UserOfChat> UserOfChats { get; set; }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Messenger.DAL.Enums;

public class UserContact
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ContactId { get; set; }
    
    public int PersonalProfileId { get; set; }
    
    public int ContactProfileId { get; set; }
    
    public Profile PersonalProfile { get; set; }
    public Profile ContactProfile { get; set; }

}
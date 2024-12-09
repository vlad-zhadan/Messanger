using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Messenger.DAL.Enums;

public class UserContact
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ContactID { get; set; }
    
    public int PersonalProfileID { get; set; }
    
    public int ContactProfileID { get; set; }
    
    public Profile PersonalProfile { get; set; }
    public Profile ContactProfile { get; set; }

}
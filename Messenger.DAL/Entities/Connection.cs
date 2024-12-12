using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Messenger.DAL.Entities;

public class Connection
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ConnectionId { get; set; }
    
    public int ProfileId { get; set; }
    public Profile Profile { get; set; }
    
    [MaxLength(100)]
    public string ConnectionString { get; set; }
}
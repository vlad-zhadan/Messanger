using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Messenger.DAL.Entities;

public class User : IdentityUser<int>
{
    [Required]
    public Profile Profile { get; set; }
}
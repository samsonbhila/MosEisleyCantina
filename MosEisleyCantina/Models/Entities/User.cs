using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

public class User : IdentityUser
{
    [Required]
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}

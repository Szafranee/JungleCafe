using System.ComponentModel.DataAnnotations;

namespace JungleCafe.Server.RequestModels;

public class LoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}
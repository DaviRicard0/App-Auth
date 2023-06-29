using System.ComponentModel.DataAnnotations;

namespace Auth.API.Models.DTOs;

public class UserRegisterRequestDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
}

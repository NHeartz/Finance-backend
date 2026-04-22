using System.ComponentModel.DataAnnotations;

namespace Finance.Api.DTOs;

public class AdminResetPasswordDto
{
    [Required] public int UserId { get; set; }
    [Required] public string NewPassword { get; set; } = null!;
}
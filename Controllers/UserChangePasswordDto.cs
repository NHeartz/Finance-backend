using System.ComponentModel.DataAnnotations;

namespace Finance.Api.DTOs;

public class UserChangePasswordDto
{
    [Required] public string OldPassword { get; set; } = null!;
    [Required] public string NewPassword { get; set; } = null!;
}
using System.ComponentModel.DataAnnotations;

namespace Finance.Api.DTOs;

public class UserUpdateRoleDto
{
    [Required] public string Role { get; set; } = null!;
}
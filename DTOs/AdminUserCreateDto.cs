using System.ComponentModel.DataAnnotations;

namespace Finance.Api.DTOs;

public class AdminUserCreateDto
{
    [Required] public string Username { get; set; } = null!;
    [Required] public string Password { get; set; } = null!;
    [Required] public string Role { get; set; } = "User"; // กำหนดสิทธิ์ได้
}
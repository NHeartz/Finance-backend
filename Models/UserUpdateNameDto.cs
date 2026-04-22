using System.ComponentModel.DataAnnotations;

namespace Finance.Api.DTOs;

public class UserUpdateNameDto
{
    [Required] public string DisplayName { get; set; } = null!;
}
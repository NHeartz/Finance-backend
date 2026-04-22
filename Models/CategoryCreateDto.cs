using System.ComponentModel.DataAnnotations;

namespace Finance.Api.DTOs;

public class CategoryCreateDto
{
    [Required] public string Name { get; set; } = null!;
    public string? Icon { get; set; }
}
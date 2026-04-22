using System.ComponentModel.DataAnnotations;

namespace Finance.Api.Models;

public class Category
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = null!;
    public string? Icon { get; set; }
    public int? UserId { get; set; }
}
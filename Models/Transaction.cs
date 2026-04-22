using System.ComponentModel.DataAnnotations;

namespace Finance.Api.Models;

public class Transaction
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    [Required]
    public string Type { get; set; } = null!; // "income" or "expense"
    public int? CategoryId { get; set; }
    public int? UserId { get; set; }
}
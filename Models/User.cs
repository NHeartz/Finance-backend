using System.Collections.Generic;

namespace Finance.Api.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string? DisplayName { get; set; }
    public string Role { get; set; } = "User"; // ค่าเริ่มต้นคือ User ทั่วไป
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
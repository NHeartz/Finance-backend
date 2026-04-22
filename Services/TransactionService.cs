using Finance.Api.Data;
using Finance.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Finance.Api.Services;

public class TransactionService : ITransactionService
{
    private readonly AppDbContext _context;

    public TransactionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync(int userId)
    {
        return await _context.Transactions
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.Date)
            .ThenByDescending(t => t.Id)
            .ToListAsync();
    }

    public async Task<Transaction> CreateTransactionAsync(Transaction transaction, int userId)
    {
        // ถ้าส่งวันที่มาให้ตั้งเป็น UTC (เพื่อให้เข้ากับ PostgreSQL) ถ้าไม่ได้ส่งมาให้ใช้วันที่ปัจจุบัน
        transaction.Date = transaction.Date == default ? DateTime.UtcNow : DateTime.SpecifyKind(transaction.Date, DateTimeKind.Utc);
        transaction.UserId = userId; // ผูกรายการนี้กับ User ที่ล็อกอิน
        
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
        return transaction;
    }

    public async Task<bool> DeleteTransactionAsync(int id, int userId)
    {
        // ค้นหารายการจาก ID และต้องเป็นของ User คนที่กำลังล็อคอินอยู่ด้วย
        var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        if (transaction == null) return false;

        // ลบออกจากฐานข้อมูล
        _context.Transactions.Remove(transaction);
        await _context.SaveChangesAsync();
        return true;
    }
}
using Finance.Api.Models;

namespace Finance.Api.Services;

public interface ITransactionService
{
    Task<IEnumerable<Transaction>> GetAllTransactionsAsync(int userId);
    Task<Transaction> CreateTransactionAsync(Transaction transaction, int userId);
    Task<bool> DeleteTransactionAsync(int id, int userId);
}
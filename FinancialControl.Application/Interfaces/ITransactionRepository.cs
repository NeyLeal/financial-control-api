using FinancialControl.Domain.Entities;

namespace FinancialControl.Application.Interfaces
{
    public interface ITransactionRepository
    {
        Task<List<Transaction>> GetByUserIdAsync(Guid userId);
        Task<Transaction?> GetByIdAsync(Guid id, Guid userId);
        Task AddAsync(Transaction transaction);
        Task UpdateAsync(Transaction transaction);
        Task DeleteAsync(Transaction transaction);
        Task SaveChangesAsync();
    }
}

using FinancialControl.Domain.Entities;
using FinancialControl.Domain.Enums;

namespace FinancialControl.Application.Interfaces
{
    public interface ITransactionRepository
    {
        Task<List<Transaction>> GetDashboardDataAsync(Guid userId, DateTime fromDate);
        Task<List<Transaction>> GetSummaryAsync(
            Guid userId,
            Guid? categoryId,
            DateTime? startDate,
            DateTime? endDate
            );
        Task<(List<Transaction>, int)> GetByUserIdAsync(
            Guid userId,
            int page,
            int pageSize,
            TransactionType? type,
            Guid? categoryId,
            DateTime? startDate,
            DateTime? endDate
            );
        Task<Transaction?> GetByIdAsync(Guid id, Guid userId);
        Task AddAsync(Transaction transaction);
        Task UpdateAsync(Transaction transaction);
        Task DeleteAsync(Transaction transaction);
        Task SaveChangesAsync();
    }
}

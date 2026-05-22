using FinancialControl.Application.Interfaces;
using FinancialControl.Domain.Entities;
using FinancialControl.Domain.Enums;
using FinancialControl.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinancialControl.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _context;

        public TransactionRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Transaction>> GetDashboardDataAsync(Guid userId, DateTime fromDate)
        {
            return await _context.Transactions
                .Include(t => t.Category)
                .Where(t => 
                t.UserId == userId &&
                t.Date >= fromDate)
                .ToListAsync();
        }
        public async Task<List<Transaction>> GetSummaryAsync(
            Guid userId,
            Guid? categoryId,
            DateTime? startDate,
            DateTime? endDate)
        {
            var query = _context.Transactions
                .Where(t => t.UserId == userId);

            if (categoryId.HasValue)
                query = query.Where(t => t.CategoryId == categoryId);
            if(startDate.HasValue)
                query = query.Where(t => t.Date >= startDate);
            if(endDate.HasValue)
                query = query.Where(t => t.Date <= endDate);

            return await query.ToListAsync();
        }
        public async Task<(List<Transaction>, int)> GetByUserIdAsync(
            Guid userId,
            int page,
            int pageSize,
            TransactionType? type,
            Guid? categoryId,
            DateTime? startDate,
            DateTime? endDate
            )
        {
            var query = _context.Transactions
                .Include(t => t.Category)
                .Where(t => t.UserId == userId);

            if (type.HasValue)
                query = query.Where(t => t.Type == type);
            if(categoryId.HasValue)
                query = query.Where(t => t.Category.Id == categoryId);
            if(startDate.HasValue)
                query = query.Where(t => t.Date >= startDate);
            if (endDate.HasValue)
                query = query.Where(t => t.Date >= endDate);

            var totalItems = await query.CountAsync();

            var data = await query
                .OrderByDescending(t => t.Date)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (data, totalItems);
        }
        public async Task<Transaction?> GetByIdAsync(Guid id, Guid userId)
        {
            return await _context.Transactions
                .FirstOrDefaultAsync(
                t => t.Id == id && 
                t.UserId == userId);
        }
        public async Task AddAsync(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
        }

        public Task UpdateAsync (Transaction transaction)
        {
            _context.Transactions.Update(transaction);

            return Task.CompletedTask;
        }
        public Task DeleteAsync(Transaction transaction)
        {
            _context.Transactions.Remove(transaction);

            return Task.CompletedTask;
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

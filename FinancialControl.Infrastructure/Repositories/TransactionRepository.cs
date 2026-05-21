using FinancialControl.Application.Interfaces;
using FinancialControl.Domain.Entities;
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

        public async Task<List<Transaction>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Transactions
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.Date)
                .ToListAsync();
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

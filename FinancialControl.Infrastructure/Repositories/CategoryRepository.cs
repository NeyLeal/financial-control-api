 using FinancialControl.Application.Interfaces;
using FinancialControl.Infrastructure.Data;
using FinancialControl.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace FinancialControl.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Categories
                .Where(c => c.UserId == userId)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(Guid id, Guid userId)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c =>  
                c.UserId == userId && 
                c.Id == id);
        }
        public async Task AddAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
        }
        public Task UpdateAsync(Category category)
        {

            return Task.CompletedTask;
        }

        public Task DeleteAsync(Category category)
        {
            _context.Categories.Remove(category);

            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

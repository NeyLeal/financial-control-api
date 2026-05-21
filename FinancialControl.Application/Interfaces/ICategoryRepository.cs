using FinancialControl.Domain.Entities;
namespace FinancialControl.Application.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetByUserIdAsync(Guid userId);
        Task<Category?> GetByIdAsync(Guid id, Guid userId);
        Task AddAsync(Category category);
        Task DeleteAsync(Category category);
        Task UpdateAsync(Category category);
        Task SaveChangesAsync();
    }
}

using FinancialControl.Application.Interfaces;
using FinancialControl.Domain.Entities;

namespace FinancialControl.Application.UseCases
{
    public class CreateCategoryUseCase
    {
        private readonly ICategoryRepository _repository;

        public CreateCategoryUseCase(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<Category> ExecuteAsync(Guid userId, string name)
        {
            var category = new Category(userId, name);

            await _repository.AddAsync(category);
            await _repository.SaveChangesAsync();

            return category;
        }
    }
}

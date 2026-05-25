using FinancialControl.Application.Exceptions;
using FinancialControl.Application.Interfaces;

namespace FinancialControl.Application.UseCases
{
    public class DeleteCategoryUseCase
    {
        private readonly ICategoryRepository _repository;

        public DeleteCategoryUseCase(
            ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> ExecuteAsync(
            Guid id,
            Guid userId)
        {
            var category =
                await _repository.GetByIdAsync(
                    id,
                    userId);

            if (category is null)
                return false;

            await _repository.DeleteAsync(category);

            await _repository.SaveChangesAsync();

            return true;
        }
    }
}
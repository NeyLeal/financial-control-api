using FinancialControl.Application.Interfaces;

namespace FinancialControl.Application.UseCases
{
    public class UpdateCategoryUseCase
    {
        private readonly ICategoryRepository _repository;

        public UpdateCategoryUseCase(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> ExecuteAsync(Guid Categoryid, Guid userId, string name)
        {
            var category = await _repository.GetByIdAsync(Categoryid, userId);
            if (category is null)
                return false;

            category.Update(name);
            await _repository.UpdateAsync(category);
            await _repository.SaveChangesAsync();
            return true;
        }
    }
}

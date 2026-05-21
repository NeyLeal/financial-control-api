using FinancialControl.Application.Interfaces;
using FinancialControl.Domain.Entities;

namespace FinancialControl.Application.UseCases
{
    public class GetCategoryUseCase
    {
        private readonly ICategoryRepository _repository;

        public GetCategoryUseCase(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Category>> ExecutAsync(Guid userId)
        {
            return await _repository.GetByUserIdAsync(userId);
        }
    }
}

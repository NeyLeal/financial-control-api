using FinancialControl.Application.Interfaces;
using FinancialControl.Domain.Entities;

namespace FinancialControl.Application.UseCases
{
    public class GetTransactionUseCase
    {
        private ITransactionRepository _repository;
        public GetTransactionUseCase(ITransactionRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Transaction>> ExecuteAsync(Guid userId)
        {
            return await _repository.GetByUserIdAsync(userId);
        }
    }
}

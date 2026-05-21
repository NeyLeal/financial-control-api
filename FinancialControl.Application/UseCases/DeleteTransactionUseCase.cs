using FinancialControl.Application.Interfaces;

namespace FinancialControl.Application.UseCases
{
    public class DeleteTransactionUseCase
    {
        private readonly ITransactionRepository _repository;
        public DeleteTransactionUseCase(ITransactionRepository repository)
        {
            _repository = repository;
        }
        public async Task<bool> ExecuteAsync(Guid id, Guid userId)
        {
            var transaction = await _repository.GetByIdAsync(id, userId);

            if(transaction is null)
                return false;

            await _repository.DeleteAsync(transaction);
            await _repository.SaveChangesAsync();

            return true;
        }
    }
}

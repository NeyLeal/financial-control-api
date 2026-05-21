using FinancialControl.Application.Interfaces;
using FinancialControl.Domain.Enums;

namespace FinancialControl.Application.UseCases
{
    public class UpdateTransactionUseCase
    {
        private readonly ITransactionRepository _repository;

        public UpdateTransactionUseCase(ITransactionRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> ExecuteAsync(
            Guid transactionId,
            Guid userId,
            decimal amount,
            string description,
            TransactionType type
            )
        {
            var transaction = await _repository.GetByIdAsync(transactionId, userId);
            if(transaction == null )
                return false;
            transaction.Update( amount, description, type );
            await _repository.UpdateAsync(transaction);
            await _repository.SaveChangesAsync();

            return true;
        }
    }
}

using FinancialControl.Application.Interfaces;
using FinancialControl.Domain.Entities;

using FinancialControl.Domain.Enums;

namespace FinancialControl.Application.UseCases
{
    public class CreateTransactionUseCase
    {
        private readonly ITransactionRepository _repository;

        public CreateTransactionUseCase(ITransactionRepository repository)
        {
            _repository = repository;
        }
        public async Task<Transaction> ExecuteAsync(
            Guid userId,
            Guid categoryId,
            decimal amount,
            string description, 
            TransactionType type
            )
        {
            var transaction = new Transaction(userId, categoryId, amount, description, type, DateTime.UtcNow);

            await _repository.AddAsync(transaction);

            await _repository.SaveChangesAsync();

            return transaction;
        }
    }
}

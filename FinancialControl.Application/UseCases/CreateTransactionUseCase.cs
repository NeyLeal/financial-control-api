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
        public async Task<Transaction> ExecuteAsync(Guid userId, decimal amount, string description, TransactionType type)
        {
            var transaction = new Transaction(userId, amount, description, type, DateTime.UtcNow);

            await _repository.AddAsync(transaction);

            await _repository.SaveChangesAsync();

            return transaction;
        }
    }
}

using FinancialControl.Application.DTOs.TransactionDto;
using FinancialControl.Application.Interfaces;
using FinancialControl.Domain.Enums;


namespace FinancialControl.Application.UseCases
{
    public class GetTransactionSummaryUseCase
    {
        private readonly ITransactionRepository _repository;
        public GetTransactionSummaryUseCase(ITransactionRepository repository)
        {
            _repository = repository;
        }

        public async Task<TransactionSummaryDto> ExecuteAsync(Guid userId)
        {
            var transactions = await _repository.GetByUserIdAsync(userId);

            var income = transactions
                .Where(t => t.Type == TransactionType.Income)
                .Sum(t => t.Amount);

            var expenses = transactions
                .Where(t => t.Type == TransactionType.Expense)
                .Sum(t => t.Amount);

            return new TransactionSummaryDto
            {
                Income = income,
                Expenses = expenses,
                Balance = income - expenses
            };
        }
    }
}

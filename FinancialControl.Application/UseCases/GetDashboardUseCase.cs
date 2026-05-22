using FinancialControl.Application.DTOs.TransactionDto;
using FinancialControl.Application.Interfaces;
using FinancialControl.Domain.Enums;

namespace FinancialControl.Application.UseCases
{
    public class GetDashboardUseCase
    {
        private readonly ITransactionRepository _repository;

        public GetDashboardUseCase(ITransactionRepository repository)
        {
            _repository = repository;
        }
        public async Task<DashboardDto> ExecuteAsync(Guid userId)
        {
            var now = DateTime.UtcNow;

            var transactions = await _repository.GetDashboardDataAsync(userId, now.AddDays(-30));
            var month = transactions.Where(x => x.Date.Month == now.Month);
            var income = month
                .Where(x => x.Type == Domain.Enums.TransactionType.Income)
                .Sum(x => x.Amount);
            var expense = month
                .Where(x => x.Type == Domain.Enums.TransactionType.Expense)
                .Sum(x => x.Amount);
            return new DashboardDto
            {
                CurrentMonthIncome = income,
                CurrentMonthExpenses = expense,
                CurrentMonthBalance = income - expense,
                Last30DaysIncome = transactions.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount),
                Last30DaysExpenses = transactions.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount),
                TopCategories =
                transactions
                .Where(t => t.Category != null)
                .GroupBy(t => t.Category!.Name)
                .Select(g => new TopCategoryDto
                {
                    Category = g.Key,
                    Amount = g.Sum(x => x.Amount)
                })
                .OrderByDescending(x => x.Amount)
                .Take(5)
                .ToList()
            };
        }
    }
}

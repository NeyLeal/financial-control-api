using FluentAssertions;
using Moq;

using FinancialControl.Application.DTOs;
using FinancialControl.Application.Interfaces;
using FinancialControl.Application.UseCases;

using FinancialControl.Domain.Entities;
using FinancialControl.Domain.Enums;

namespace FinancialControl.Tests.UseCases
{
    public class GetDashboardUseCaseTests
    {
        private readonly Mock<ITransactionRepository> _repository;
        private readonly GetDashboardUseCase _dashboardUseCase;

        public GetDashboardUseCaseTests()
        {
            _repository = new Mock<ITransactionRepository>();
            _dashboardUseCase = new GetDashboardUseCase(_repository.Object);
        }

        [Fact]
        public async Task ExecuteAsync_WhenUserHasTransactions_ShouldCalculateDashboardCorrectly()
        {
            var userId = Guid.NewGuid();

            var transactions = new List<Transaction>
            {
                new Transaction(
                    userId,
                    Guid.NewGuid(),
                    5000,
                    "Salário",
                    TransactionType.Income,
                    DateTime.UtcNow
                ),

                new Transaction(
                    userId,
                    Guid.NewGuid(),
                    1500,
                    "Conta",
                    TransactionType.Expense,
                    DateTime.UtcNow
                ),

                new Transaction(
                    userId,
                    Guid.NewGuid(),
                    500,
                    "Mercado",
                    TransactionType.Expense,
                    DateTime.UtcNow
                )
            };

            _repository
            .Setup(r =>
                r.GetDashboardDataAsync(
                    userId,
                    It.IsAny<DateTime>()
                ))
            .ReturnsAsync(transactions);

            var result = await _dashboardUseCase.ExecuteAsync(userId);

            // Assert

            result.CurrentMonthIncome.Should().Be(5000);

            result.CurrentMonthExpenses.Should().Be(2000);

            result.CurrentMonthBalance.Should().Be(3000);
        }

        [Fact]
        public async Task ExecuteAsync_WhenUserHasNoTransactions_ShouldReturnZeroValues()
        {
            var userId = Guid.NewGuid();

            _repository.Setup(r =>
                r.GetDashboardDataAsync(userId, It.IsAny<DateTime>()))
                .ReturnsAsync(new List<Transaction>());

            var result = await _dashboardUseCase.ExecuteAsync(userId);

            result.CurrentMonthIncome.Should().Be(0);
            result.CurrentMonthExpenses.Should().Be(0);
            result.CurrentMonthBalance.Should().Be(0);
            result.Last30DaysIncome.Should().Be(0);
            result.Last30DaysExpenses.Should().Be(0);
            result.TopCategories.Should().BeEmpty();
        }

        
    }
}

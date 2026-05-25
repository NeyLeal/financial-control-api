using FinancialControl.Application.DTOs.TransactionDto;
using FinancialControl.Application.Interfaces;
using FinancialControl.Application.UseCases;
using FinancialControl.Domain.Entities;
using FinancialControl.Domain.Enums;
using FluentAssertions;
using Moq;

namespace FinancialControl.Tests.UseCases
{
    public class GetTransactionSummaryUseCaseTests
    {
        private readonly Mock<ITransactionRepository> _repository;
        private readonly GetTransactionSummaryUseCase _useCase;

        public GetTransactionSummaryUseCaseTests()
        {
            _repository = new Mock<ITransactionRepository>();

            _useCase = new GetTransactionSummaryUseCase(
                _repository.Object
            );
        }

        [Fact]
        public async Task ExecuteAsync_WhenUserHasTRansactions_ShuldCalculateSummary()
        {
            var userId = Guid.NewGuid();
            var transactions = new List<Transaction>
            {
                new Transaction(
                userId,
                Guid.NewGuid(),
                5000,
                "Salario",
                TransactionType.Income,
                DateTime.UtcNow
            ),
                new Transaction(
                    userId,
                    Guid.NewGuid(),
                    2000,
                    "Mercado",
                    TransactionType.Expense,
                    DateTime.UtcNow
                ),

                new Transaction(
                    userId,
                    Guid.NewGuid(),
                    1000,
                    "Academia",
                    TransactionType.Expense,
                    DateTime.UtcNow
                )
            };

            _repository
            .Setup(r => r.GetSummaryAsync(
                userId,
                It.IsAny<Guid?>(),
                It.IsAny<DateTime?>(),
                It.IsAny<DateTime?>()))
            .ReturnsAsync(transactions);

            var result = await _useCase.ExecuteAsync(
                 userId,
                 new GetTransactionSummaryDto()
            );

            result.Income.Should().Be(5000);

            result.Expenses.Should().Be(3000);

            result.Balance.Should().Be(2000);
        }
    }
}

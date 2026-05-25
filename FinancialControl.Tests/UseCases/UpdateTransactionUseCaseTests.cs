using FinancialControl.Application.Interfaces;
using FinancialControl.Application.UseCases;

using FinancialControl.Domain.Entities;
using FinancialControl.Domain.Enums;

using FluentAssertions;

using Moq;

namespace FinancialControl.Tests.UseCases
{
    public class UpdateTransactionUseCaseTests
    {
        private readonly Mock<ITransactionRepository> _repository;
        private readonly UpdateTransactionUseCase _useCase;

        public UpdateTransactionUseCaseTests()
        {
            _repository = new Mock<ITransactionRepository>();
            _useCase = new UpdateTransactionUseCase(_repository.Object);
        }

        [Fact]
        public async Task ExecuteAsync_WhenTransactionEcxistis_ShouldUpdate()
        {
            var userId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();
            var transaction = new Transaction(
                userId,
                categoryId,
                100,
                "Original",
                TransactionType.Expense,
                DateTime.UtcNow
                );
            _repository
                .Setup(x => x.GetByIdAsync(transaction.Id, userId))
                .ReturnsAsync( transaction );

            var result = await _useCase.ExecuteAsync(transaction.Id, userId, categoryId, 700, "Atualizado", TransactionType.Income);

            result.Should().BeTrue();

            transaction.Amount
                .Should()
                .Be(700);
            transaction.Description.Should().Be("Atualizado");
            transaction.Type.Should().Be(TransactionType.Income);
            _repository
                .Verify(x => x.SaveChangesAsync(),
                Times.Once());
        }

        [Fact]
        public async Task ExecuteAsync_WhenTransactionDoesNotExist_ShouldRetunFalse()
        {
            var transationId = Guid.NewGuid(); 

            var userId = Guid.NewGuid();

            _repository
                .Setup(x => x.GetByIdAsync(transationId, userId))
                .ReturnsAsync((Transaction?)null);

            var result = await _useCase.ExecuteAsync(transationId, userId, Guid.NewGuid(), 500, "Teste", TransactionType.Expense);

            result.Should().BeFalse();

            _repository
                .Verify(x => x.SaveChangesAsync(),
                Times.Never());
        }
    }
}
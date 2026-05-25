using FinancialControl.Application.Interfaces;
using FinancialControl.Application.UseCases;

using FinancialControl.Domain.Entities;
using FinancialControl.Domain.Enums;
using FluentAssertions;
using Moq;

namespace FinancialControl.Tests.UseCases;

public class CreateTransactionUseCaseTests
{
    private readonly Mock<ITransactionRepository> _transactionRepository;

    private readonly CreateTransactionUseCase _useCase;

    public CreateTransactionUseCaseTests()
    {
        _transactionRepository =
            new Mock<ITransactionRepository>();

        _useCase =
            new CreateTransactionUseCase(
                _transactionRepository.Object
            );
    }

    [Fact]
    public async Task ExecuteAsync_WhenDataIsValid_ShouldCreateTransaction()
    {
        // Arrange

        var userId = Guid.NewGuid();

        var categoryId = Guid.NewGuid();

        // Act

        var result =
            await _useCase.ExecuteAsync(
                userId,
                categoryId,
                500,
                "Mercado",
                TransactionType.Expense
            );

        // Assert

        result.Should().NotBeNull();

        result.UserId.Should().Be(userId);

        result.CategoryId.Should().Be(categoryId);

        result.Amount.Should().Be(500);

        result.Description.Should().Be("Mercado");

        result.Type.Should().Be(TransactionType.Expense);

        _transactionRepository
            .Verify(
                x => x.AddAsync(
                    It.IsAny<Transaction>()),
                Times.Once);

        _transactionRepository
            .Verify(
                x => x.SaveChangesAsync(),
                Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenRepositoryFails_ShouldNotSaveChanges()
    {
        var userId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();
        _transactionRepository
            .Setup(x => x.AddAsync(It.IsAny<Transaction>()))
            .ThrowsAsync(new Exception("Database error"));

        var action = async () => await _useCase.ExecuteAsync(userId, categoryId, 500, "Mercado", TransactionType.Expense);

        await action
            .Should()
            .ThrowAsync<Exception>()
            .WithMessage("Database error");

        _transactionRepository
            .Verify(x =>  x.SaveChangesAsync(), Times.Never);
    }
}
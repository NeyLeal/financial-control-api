using FinancialControl.Application.Interfaces;
using FinancialControl.Application.UseCases;

using FinancialControl.Domain.Entities;

using FluentAssertions;

using Moq;

namespace FinancialControl.Tests.UseCases;

public class UpdateCategoryUseCaseTests
{
    private readonly Mock<ICategoryRepository> _repository;

    private readonly UpdateCategoryUseCase _useCase;

    public UpdateCategoryUseCaseTests()
    {
        _repository =
            new Mock<ICategoryRepository>();

        _useCase =
            new UpdateCategoryUseCase(
                _repository.Object
            );
    }

    [Fact]
    public async Task ExecuteAsync_WhenCategoryExists_ShouldUpdate()
    {

        var userId =
            Guid.NewGuid();

        var category =
            new Category(
                userId,
                "Food"
            );

        _repository
            .Setup(
                x => x.GetByIdAsync(
                    category.Id,
                    userId))
            .ReturnsAsync(
                category);

        var result =
            await _useCase.ExecuteAsync(
                category.Id,
                userId,
                "Food Updated"
            );


        result.Should().BeTrue();

        category.Name
            .Should()
            .Be("Food Updated");

        _repository
            .Verify(
                x => x.SaveChangesAsync(),
                Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenCategoryDoesNotExist_ShouldReturnFalse()
    {

        var userId =
            Guid.NewGuid();

        var categoryId =
            Guid.NewGuid();

        _repository
            .Setup(
                x => x.GetByIdAsync(
                    categoryId,
                    userId))
            .ReturnsAsync(
                (Category?)null);


        var result =
            await _useCase.ExecuteAsync(
                categoryId,
                userId,
                "Updated"
            );


        result.Should().BeFalse();

        _repository
            .Verify(
                x => x.SaveChangesAsync(),
                Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WhenSaveFails_ShouldThrowException()
    {

        var userId =
            Guid.NewGuid();

        var category =
            new Category(
                userId,
                "Food"
            );

        _repository
            .Setup(
                x => x.GetByIdAsync(
                    category.Id,
                    userId))
            .ReturnsAsync(
                category);

        _repository
            .Setup(
                x => x.SaveChangesAsync())
            .ThrowsAsync(
                new Exception(
                    "Database error"));

 
        var action =
            async () =>
                await _useCase.ExecuteAsync(
                    category.Id,
                    userId,
                    "Updated"
                );


        await action
            .Should()
            .ThrowAsync<Exception>()
            .WithMessage(
                "Database error");
    }
}
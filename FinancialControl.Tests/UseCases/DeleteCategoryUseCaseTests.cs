using FinancialControl.Application.Exceptions;
using FinancialControl.Application.Interfaces;
using FinancialControl.Application.UseCases;

using FinancialControl.Domain.Entities;

using FluentAssertions;

using Moq;
namespace FinancialControl.Tests.UseCases
{
    public class DeleteCategoryUseCaseTests
    {
        private readonly Mock<ICategoryRepository> _repository;
        private readonly DeleteCategoryUseCase _useCase;

        public DeleteCategoryUseCaseTests()
        {
            _repository = new Mock<ICategoryRepository>();
            _useCase = new DeleteCategoryUseCase(_repository.Object);
        }

        [Fact]
        public async Task ExecuteAsync_WhenCategoryExists_ShouldDelete()
        {
            var userId = Guid.NewGuid();
            var category = new Category(userId, "Food");

            _repository
                .Setup(x => x.GetByIdAsync(category.Id, userId))
                .ReturnsAsync(category);

            var result = await _useCase.ExecuteAsync(category.Id, userId);

            result.Should().BeTrue();
            _repository
                .Verify(x =>  x.DeleteAsync(category), Times.Once());
            _repository
                .Verify(x => x.SaveChangesAsync(), Times.Once);
        }
        [Fact]
        public async Task ExecuteAsync_WhenCategoryDoesNotExist_ShouldReturnFalse()
        {
            // Arrange

            var userId = Guid.NewGuid();

            var categoryId =
                Guid.NewGuid();

            _repository
                .Setup(x =>
                    x.GetByIdAsync(
                        categoryId,
                        userId))
                .ReturnsAsync(
                    (Category?)null);

            // Act

            var result =
                await _useCase.ExecuteAsync(
                    categoryId,
                    userId);

            // Assert

            result.Should().BeFalse();

            _repository
                .Verify(
                    x => x.DeleteAsync(
                        It.IsAny<Category>()),
                    Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_WhenDeleteFails_ShouldThrowException()
        {
            // Arrange

            var userId = Guid.NewGuid();

            var category =
                new Category(
                    userId,
                    "Food"
                );

            _repository
                .Setup(x =>
                    x.GetByIdAsync(
                        category.Id,
                        userId))
                .ReturnsAsync(category);

            _repository
                .Setup(x =>
                    x.DeleteAsync(category))
                .ThrowsAsync(
                    new Exception());

            // Act

            var action =
                async () =>
                    await _useCase.ExecuteAsync(
                        category.Id,
                        userId);

            // Assert

            await action
                .Should()
                .ThrowAsync<Exception>();

            _repository
                .Verify(
                    x => x.SaveChangesAsync(),
                    Times.Never);
        }
    }
}

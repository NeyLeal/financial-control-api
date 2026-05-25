using FinancialControl.Application.Interfaces;
using FinancialControl.Application.UseCases;
using FinancialControl.Domain.Entities;
using FluentAssertions;
using Moq;

namespace FinancialControl.Tests.UseCases
{
    public class CreateCategoryUseCaseTests
    {
        private readonly Mock<ICategoryRepository> _repository;
        private readonly CreateCategoryUseCase _useCase;

        public CreateCategoryUseCaseTests()
        {
            _repository = new Mock<ICategoryRepository>();
            _useCase = new CreateCategoryUseCase(_repository.Object);
        }

        [Fact]
        public async Task ExecuteAsync_WhenDataIsValid_ShouldCreateCategory()
        {
            var userId = Guid.NewGuid();

            var result = await _useCase.ExecuteAsync(userId, "Food");

            result.Should().NotBeNull();

            result.UserId.Should().Be(userId);

            result.Name.Should().Be("Food");

            _repository
                .Verify(x => x.AddAsync(It.IsAny<Category>()), Times.Once());

            _repository
                .Verify(x => x.SaveChangesAsync(), Times.Once());
        }

        [Fact]
        public async Task ExecuteAsync_WhenRepositoryFals_ShouldThrowException()
        {
            var userId = Guid.NewGuid();

            _repository
                .Setup(x => x.AddAsync(It.IsAny<Category>()))
                .ThrowsAsync(new Exception("Database error"));

            var action = async () => await _useCase.ExecuteAsync(userId, "Food");

            await action
                .Should()
                .ThrowAsync<Exception>()
                .WithMessage("Database error");

            _repository
                .Verify(x => x.SaveChangesAsync(), Times.Never());
        }
    }
}

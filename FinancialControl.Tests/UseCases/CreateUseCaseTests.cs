using FinancialControl.Application.DTOs.UserDto;
using FinancialControl.Application.Exceptions;
using FinancialControl.Application.Interfaces;
using FinancialControl.Application.UseCases;

using FinancialControl.Domain.Entities;

using FluentAssertions;

using Moq;

namespace FinancialControl.Tests.UseCases;

public class CreateUserUseCaseTests
{
    private readonly Mock<IUserRepository> _repository;

    private readonly CreateUserUseCase _useCase;

    public CreateUserUseCaseTests()
    {
        _repository =
            new Mock<IUserRepository>();

        _useCase =
            new CreateUserUseCase(
                _repository.Object
            );
    }

    [Fact]
    public async Task Execute_WhenDataIsValid_ShouldCreateUser()
    {

        var dto =
            new CreateUserDto
            {
                Name = "Ney",
                Email = "teste@email.com",
                Password = "123456"
            };

        _repository
            .Setup(x =>
                x.GetByEmailAsync(dto.Email))
            .ReturnsAsync((User)null!);


        var result =
            await _useCase.Execute(dto);

 
        result.Should()
            .NotBeNull();

        result.Name
            .Should()
            .Be(dto.Name);

        result.Email
            .Should()
            .Be(dto.Email);

        _repository.Verify(
            x => x.AddAsync(
                It.IsAny<User>()),
            Times.Once);
    }

    [Fact]
    public async Task Execute_WhenEmailAlreadyExists_ShouldThrowBusinessException()
    {

        var dto =
            new CreateUserDto
            {
                Name = "Ney",
                Email = "teste@email.com",
                Password = "123456"
            };

        var existingUser =
            new User(
                "Teste",
                dto.Email,
                "hash"
            );

        _repository
            .Setup(x =>
                x.GetByEmailAsync(dto.Email))
            .ReturnsAsync(existingUser);

 
        Func<Task> action =
            async () =>
                await _useCase.Execute(dto);


        await action
            .Should()
            .ThrowAsync<BusinessException>();
    }
}
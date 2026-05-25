using FinancialControl.Application.DTOs.UserDto;
using FinancialControl.Application.Exceptions;
using FinancialControl.Application.Interfaces;
using FinancialControl.Application.UseCases;

using FinancialControl.Domain.Entities;

using FluentAssertions;

using Moq;

namespace FinancialControl.Tests.UseCases;

public class LoginUseCaseTests
{
    private readonly Mock<IUserRepository> _repository;

    private readonly Mock<IJwtTokenGenerator> _tokenGenerator;

    private readonly LoginUseCase _useCase;

    public LoginUseCaseTests()
    {
        _repository =
            new Mock<IUserRepository>();

        _tokenGenerator =
            new Mock<IJwtTokenGenerator>();

        _useCase =
            new LoginUseCase(
                _repository.Object,
                _tokenGenerator.Object
            );
    }

    [Fact]
    public async Task Execute_WhenCredentialsAreValid_ShouldReturnToken()
    {
        var dto =
            new LoginDto
            {
                Email = "teste@email.com",
                Password = "123456"
            };

        var user =
            new User(
                "Ney",
                dto.Email,
                BCrypt.Net.BCrypt.HashPassword(dto.Password)
            );

        _repository
            .Setup(x =>
                x.GetByEmailAsync(dto.Email))
            .ReturnsAsync(user);

        _tokenGenerator
            .Setup(x =>
                x.Generate(It.IsAny<User>()))
            .Returns("fake-jwt");

        var result =
            await _useCase.Execute(dto);

        result
            .Should()
            .Be("fake-jwt");
    }

    [Fact]
    public async Task Execute_WhenUserDoesNotExist_ShouldThrowBusinessException()
    {

        var dto =
            new LoginDto
            {
                Email = "notfound@email.com",
                Password = "123"
            };

        _repository
            .Setup(x =>
                x.GetByEmailAsync(dto.Email))
            .ReturnsAsync((User)null!);


        Func<Task> action =
            async () =>
                await _useCase.Execute(dto);


        await action
            .Should()
            .ThrowAsync<BusinessException>();
    }

    [Fact]
    public async Task Execute_WhenPasswordIsInvalid_ShouldThrowBusinessException()
    {

        var dto =
            new LoginDto
            {
                Email = "teste@email.com",
                Password = "senhaErrada"
            };

        var user =
            new User(
                "Ney",
                dto.Email,
                BCrypt.Net.BCrypt.HashPassword("123456")
            );

        _repository
            .Setup(x =>
                x.GetByEmailAsync(dto.Email))
            .ReturnsAsync(user);


        Func<Task> action =
            async () =>
                await _useCase.Execute(dto);


        await action
            .Should()
            .ThrowAsync<BusinessException>();
    }
}
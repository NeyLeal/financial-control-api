using FinancialControl.API.Controllers;
using FinancialControl.Application.DTOs.UserDto;
using FinancialControl.Application.Interfaces;
using FinancialControl.Application.UseCases;
using FinancialControl.Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FinancialControl.Tests.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IUserRepository> _repository;

    private readonly Mock<IJwtTokenGenerator> _tokenGenerator;

    private readonly LoginUseCase _useCase;

    private readonly AuthController _controller;

    public AuthControllerTests()
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

        _controller =
            new AuthController(
                _useCase
            );
    }

    [Fact]
    public async Task Login_WhenCredentialsAreValid_ShouldReturnOk()
    {
        // Arrange

        var password = "123456";

        var dto =
            new LoginDto
            {
                Email = "ney@test.com",
                Password = password
            };

        var user =
            new User(
                "Ney",
                dto.Email,
                BCrypt.Net.BCrypt.HashPassword(password)
            );

        _repository
            .Setup(x =>
                x.GetByEmailAsync(dto.Email))
            .ReturnsAsync(user);

        _tokenGenerator
            .Setup(x =>
                x.Generate(user))
            .Returns("fake-token");

        // Act

        var result =
            await _controller.Login(dto);

        // Assert

        result
            .Should()
            .BeOfType<OkObjectResult>();

        var ok =
            result as OkObjectResult;

        ok!.Value
            .Should()
            .NotBeNull();
    }
}
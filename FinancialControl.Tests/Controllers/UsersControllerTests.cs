using FinancialControl.API.Controllers;
using FinancialControl.Application.DTOs.UserDto;
using FinancialControl.Application.Interfaces;
using FinancialControl.Application.UseCases;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FinancialControl.Tests.Controllers;

public class UsersControllerTests
{
    private readonly Mock<IUserRepository> _repository;

    private readonly CreateUserUseCase _createUseCase;

    private readonly UsersController _controller;

    public UsersControllerTests()
    {
        _repository =
            new Mock<IUserRepository>();

        _createUseCase =
            new CreateUserUseCase(
                _repository.Object
            );

        _controller =
            new UsersController(
                _createUseCase
            );
    }

    [Fact]
    public async Task Create_WhenRequestIsValid_ShouldReturnOk()
    {
        // Arrange

        var dto =
            new CreateUserDto
            {
                Name = "Ney",
                Email = "ney@test.com",
                Password = "123456"
            };

        _repository
            .Setup(x =>
                x.GetByEmailAsync(dto.Email))
            .ReturnsAsync((Domain.Entities.User?)null);

        // Act

        var result =
            await _controller.Create(dto);

        // Assert

        result
            .Should()
            .BeOfType<OkObjectResult>();

        _repository
            .Verify(x =>
                x.AddAsync(
                    It.IsAny<Domain.Entities.User>()),
                Times.Once);
    }
}
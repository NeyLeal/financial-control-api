using FinancialControl.API.Controllers;
using FinancialControl.Application.DTOs.CategoryDto;
using FinancialControl.Application.Interfaces;
using FinancialControl.Application.UseCases;
using FinancialControl.Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace FinancialControl.Tests.Controllers;

public class CategoriesControllerTests
{
    private readonly Mock<ICategoryRepository> _repository;

    private readonly CategoriesController _controller;

    public CategoriesControllerTests()
    {
        _repository =
            new Mock<ICategoryRepository>();

        var create =
            new CreateCategoryUseCase(
                _repository.Object);

        var get =
            new GetCategoryUseCase(
                _repository.Object);

        var update =
            new UpdateCategoryUseCase(
                _repository.Object);

        var delete =
            new DeleteCategoryUseCase(
                _repository.Object);

        _controller =
            new CategoriesController(
                create,
                get,
                update,
                delete);

        var userId = Guid.NewGuid();

        _controller.ControllerContext =
            new ControllerContext
            {
                HttpContext =
                    new DefaultHttpContext
                    {
                        User =
                            new ClaimsPrincipal(
                                new ClaimsIdentity(
                                [
                                    new Claim(
                                    ClaimTypes.NameIdentifier,
                                    userId.ToString())
                                ]))
                    }
            };
    }

    [Fact]
    public async Task Create_WhenRequestIsValid_ShouldReturnOk()
    {
        var dto =
            new CreateCategoryDto
            {
                Name = "Food"
            };

        _repository
            .Setup(x =>
                x.AddAsync(
                    It.IsAny<Category>()))
            .Returns(Task.CompletedTask);

        _repository
            .Setup(x =>
                x.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        var result =
            await _controller.Create(dto);

        result
            .Should()
            .BeOfType<OkObjectResult>();
    }
    [Fact]
    public async Task Delete_WhenCategoryDoesNotExist_ShouldReturnNotFound()
    {
        var id =
            Guid.NewGuid();

        _repository
            .Setup(x =>
                x.GetByIdAsync(
                    id,
                    It.IsAny<Guid>()))
            .ReturnsAsync((Category?)null);

        var result =
            await _controller.Delete(id);

        result
            .Should()
            .BeOfType<NotFoundResult>();
    }
    [Fact]
    public async Task Delete_WhenCategoryExists_ShouldReturnNoContent()
    {
        var id =
            Guid.NewGuid();

        var category =
            new Category(
                Guid.NewGuid(),
                "Food");

        _repository
            .Setup(x =>
                x.GetByIdAsync(
                    id,
                    It.IsAny<Guid>()))
            .ReturnsAsync(category);

        _repository
            .Setup(x =>
                x.DeleteAsync(category))
            .Returns(Task.CompletedTask);

        _repository
            .Setup(x =>
                x.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        var result =
            await _controller.Delete(id);

        result
            .Should()
            .BeOfType<NoContentResult>();
    }
}
using FinancialControl.API.Controllers;
using FinancialControl.Application.DTOs.TransactionDto;
using FinancialControl.Application.Interfaces;
using FinancialControl.Application.UseCases;
using FinancialControl.Domain.Entities;
using FinancialControl.Domain.Enums;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace FinancialControl.Tests.Controllers;

public class TransactionsControllerTests
{
    private readonly Mock<ITransactionRepository> _transactionRepository;
    private readonly Mock<ICategoryRepository> _categoryRepository;

    private readonly TransactionsController _controller;

    public TransactionsControllerTests()
    {
        _transactionRepository =
            new Mock<ITransactionRepository>();

        _categoryRepository =
            new Mock<ICategoryRepository>();


        var create =
            new CreateTransactionUseCase(
                _transactionRepository.Object);

        var delete =
            new DeleteTransactionUseCase(
                _transactionRepository.Object);

        var get =
            new GetTransactionUseCase(
                _transactionRepository.Object);

        var summary =
            new GetTransactionSummaryUseCase(
                _transactionRepository.Object);

        var dashboard =
            new GetDashboardUseCase(
                _transactionRepository.Object);

        var update =
            new UpdateTransactionUseCase(
                _transactionRepository.Object);


        _controller =
            new TransactionsController(
                create,
                delete,
                get,
                summary,
                dashboard,
                update
            );

        var userId =
            Guid.NewGuid();

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
        var category =
            new Category(
                Guid.NewGuid(),
                "Food");

        var dto =
            new CreateTransactionDto
            {
                CategoryId = category.Id,
                Amount = 500,
                Description = "Mercado",
                Type = (int)TransactionType.Expense
            };

        _transactionRepository
            .Setup(x =>
                x.AddAsync(
                    It.IsAny<Transaction>()))
            .Returns(Task.CompletedTask);

        _transactionRepository
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
    public async Task Get_WhenRequestIsValid_ShouldReturnOk()
    {
        var dto =
            new GetTransactionDto();

        _transactionRepository
            .Setup(x =>
                x.GetByUserIdAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<TransactionType?>(),
                    It.IsAny<Guid?>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<DateTime?>()))
            .ReturnsAsync(
                (
                    new List<Transaction>(),
                    0
                ));

        var result =
            await _controller.Get(dto);

        result
            .Should()
            .BeOfType<OkObjectResult>();
    }
    [Fact]
    public async Task Summary_WhenRequestIsValid_ShouldReturnOk()
    {
        var dto =
            new GetTransactionSummaryDto();

        _transactionRepository
            .Setup(x =>
                x.GetSummaryAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<Guid?>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<DateTime?>()))
            .ReturnsAsync(
                new List<Transaction>());

        var result =
            await _controller.GetSummary(dto);

        result
            .Should()
            .BeOfType<OkObjectResult>();
    }
    [Fact]
    public async Task Dashboard_WhenRequestIsValid_ShouldReturnOk()
    {
        _transactionRepository
            .Setup(x =>
                x.GetDashboardDataAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<DateTime>()))
            .ReturnsAsync(
                new List<Transaction>());

        var result =
            await _controller.Dashboard();

        result
            .Should()
            .BeOfType<OkObjectResult>();
    }
    [Fact]
    public async Task Update_WhenTransactionExists_ShouldReturnNoContent()
    {
        var id =
            Guid.NewGuid();

        var dto =
            new UpdateTransactionDto
            {
                CategoryId = Guid.NewGuid(),
                Amount = 800,
                Description = "Mercado",
                Type = (int)TransactionType.Expense
            };

        var transaction =
            new Transaction(
                Guid.NewGuid(),
                dto.CategoryId,
                500,
                "Old",
                TransactionType.Expense,
                DateTime.UtcNow);

        _transactionRepository
            .Setup(x =>
                x.GetByIdAsync(
                    id,
                    It.IsAny<Guid>()))
            .ReturnsAsync(transaction);

        _transactionRepository
            .Setup(x =>
                x.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        var result =
            await _controller.Update(
                id,
                dto);

        result
            .Should()
            .BeOfType<NoContentResult>();
    }
    [Fact]
    public async Task Update_WhenTransactionDoesNotExist_ShouldReturnNotFound()
    {
        var id =
            Guid.NewGuid();

        var dto =
            new UpdateTransactionDto
            {
                CategoryId = Guid.NewGuid(),
                Amount = 800,
                Description = "Mercado",
                Type = (int)TransactionType.Expense
            };

        _transactionRepository
            .Setup(x =>
                x.GetByIdAsync(
                    id,
                    It.IsAny<Guid>()))
            .ReturnsAsync((Transaction?)null);

        var result =
            await _controller.Update(
                id,
                dto);

        result
            .Should()
            .BeOfType<NotFoundObjectResult>();
    }
    [Fact]
    public async Task Delete_WhenTransactionExists_ShouldReturnNoContent()
    {
        var id =
            Guid.NewGuid();

        var transaction =
            new Transaction(
                Guid.NewGuid(),
                Guid.NewGuid(),
                500,
                "Mercado",
                TransactionType.Expense,
                DateTime.UtcNow);

        _transactionRepository
            .Setup(x =>
                x.GetByIdAsync(
                    id,
                    It.IsAny<Guid>()))
            .ReturnsAsync(transaction);

        _transactionRepository
            .Setup(x =>
                x.DeleteAsync(transaction))
            .Returns(Task.CompletedTask);

        _transactionRepository
            .Setup(x =>
                x.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        var result =
            await _controller.Delete(id);

        result
            .Should()
            .BeOfType<NoContentResult>();
    }
    [Fact]
    public async Task Delete_WhenTransactionDoesNotExist_ShouldReturnNotFound()
    {
        var id =
            Guid.NewGuid();

        _transactionRepository
            .Setup(x =>
                x.GetByIdAsync(
                    id,
                    It.IsAny<Guid>()))
            .ReturnsAsync((Transaction?)null);

        var result =
            await _controller.Delete(id);

        result
            .Should()
            .BeOfType<NotFoundObjectResult>();
    }
}
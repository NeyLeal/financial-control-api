using FinancialControl.Application.DTOs;
using FinancialControl.Domain.Entities;
using FinancialControl.Domain.Enums;
using FinancialControl.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace FinancialControl.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class TransactionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TransactionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var transactions = _context.Transactions
                .Where(t => t.UserId == Guid.Parse(userId))
                .OrderByDescending(t => t.Date)
                .ToList();

            return Ok(transactions);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateTransactionDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var transaction = new Transaction(
                Guid.Parse(userId!),
                dto.Amount,
                dto.Description,
                (Domain.Enums.TransactionType)dto.Type,
                DateTime.UtcNow
            );
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return Ok(transaction);
        }
        [HttpGet("summary")]
        public IActionResult GetSummary()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var transactions = _context.Transactions
                .Where(t => t.UserId == Guid.Parse(userId!))
                .OrderByDescending(t => t.Date)
                .ToList();

            var income = transactions
                .Where(t => t.Type == TransactionType.Income)
                .Sum(t => t.Amount);

            var expenses = transactions
                .Where(t => t.Type == TransactionType.Expense)
                .Sum(t => t.Amount);

            var summary = new TransactionSummaryDto
            {
                Income = income,
                Expenses = expenses,
                Balance = income - expenses

            };

            return Ok(summary);
        }
    }
}

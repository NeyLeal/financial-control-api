using FinancialControl.Application.DTOs;
using FinancialControl.Domain.Entities;
using FinancialControl.Domain.Enums;
using FinancialControl.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        
        #region Get
        [HttpGet]
        public IActionResult Get()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var transactions = _context.Transactions
                .Where(t => t.UserId == Guid.Parse(userId!))
                .OrderByDescending(t => t.Date)
                .ToList();

            return Ok(transactions);
        }
        [HttpGet("summary")]
        public IActionResult GetSummary()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var transactions = _context.Transactions
                .Where(t => t.UserId == Guid.Parse(userId!))
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
        #endregion
        #region Post
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
        #endregion
        #region Put
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateTransactionDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var transaction = await _context.Transactions.FirstOrDefaultAsync(
                t => t.Id == id && 
                t.UserId == Guid.Parse(userId!));

            if(transaction == null)
            {
                return NotFound(new
                {
                    message = "Transaction not found."
                });
            }

            transaction.Update( dto.Amount, dto.Description, (TransactionType)dto.Type);

            await _context.SaveChangesAsync();

            return Ok(transaction);
        }
        #endregion
        #region Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.Id == id && t.UserId == Guid.Parse(userId!));

            if(transaction is null)
            {
                return NotFound(new
                {
                    message = "Transaction not found."
                });
            }
            _context.Transactions.Remove(transaction);

            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion

    }
}

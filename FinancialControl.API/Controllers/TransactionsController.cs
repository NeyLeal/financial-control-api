using FinancialControl.Application.DTOs.TransactionDto;
using FinancialControl.Application.UseCases;
using FinancialControl.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinancialControl.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class TransactionsController : ControllerBase
    {
        private readonly GetTransactionUseCase _getTransactionUseCase;
        private readonly GetTransactionSummaryUseCase _getTransactionSummaryUseCase;
        private readonly CreateTransactionUseCase _createTransactionUseCase;
        private readonly DeleteTransactionUseCase _deleteTransactionUseCase;
        private readonly UpdateTransactionUseCase _updateTransactionUseCase;
        private readonly GetDashboardUseCase _getDashboardUseCase;

        public TransactionsController(
            CreateTransactionUseCase createTransactionUseCase,
            DeleteTransactionUseCase deleteTransactionUseCase,
            GetTransactionUseCase getTransactionUseCase,
            GetTransactionSummaryUseCase getTransactionSummaryUseCase,
            GetDashboardUseCase getDashboardUseCase,
            UpdateTransactionUseCase updateTransactionUseCase
            )
        {
            _getTransactionUseCase = getTransactionUseCase;
            _getTransactionSummaryUseCase = getTransactionSummaryUseCase;
            _createTransactionUseCase = createTransactionUseCase;
            _deleteTransactionUseCase = deleteTransactionUseCase;
            _updateTransactionUseCase = updateTransactionUseCase;
            _getDashboardUseCase = getDashboardUseCase; 
        }

        
        #region Get
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetTransactionDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var transaction = await _getTransactionUseCase.ExecuteAsync(Guid.Parse(userId!), dto);

            return Ok(transaction);
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary([FromQuery] GetTransactionSummaryDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var summary = await _getTransactionSummaryUseCase.ExecuteAsync(Guid.Parse(userId!), dto);

            return Ok(summary);
        }
        [HttpGet("dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _getDashboardUseCase.ExecuteAsync(userId);
            return Ok(result);
        }
        #endregion
        #region Post
        [HttpPost]
        public async Task<IActionResult> Create(CreateTransactionDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var transaction = await _createTransactionUseCase.ExecuteAsync(
                Guid.Parse(userId!),
                dto.CategoryId,
                dto.Amount,
                dto.Description,
                (TransactionType)dto.Type);

            return Ok(transaction);
        }
        #endregion
        #region Put
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateTransactionDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var updated = await _updateTransactionUseCase.ExecuteAsync(
                        id,
                        Guid.Parse(userId!),
                        dto.CategoryId,
                        dto.Amount,
                        dto.Description,
                        (TransactionType)dto.Type);

            if (!updated)
            {
                return NotFound(new
                {
                    message = "Transaction not found."
                });
            }

            return NoContent();
        }
        #endregion
        #region Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var delete = await _deleteTransactionUseCase.ExecuteAsync(id, Guid.Parse(userId!));

            if(!delete)
            {
                return NotFound(new
                {
                    message = "Transaction not found."
                });
            }

            return NoContent();
        }
        #endregion

    }
}

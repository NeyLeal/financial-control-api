using FinancialControl.Application.DTOs.TransactionDto;
using FinancialControl.Application.Interfaces;
using FinancialControl.Domain.Enums;

namespace FinancialControl.Application.UseCases
{
    public class GetTransactionUseCase
    {
        private ITransactionRepository _repository;
        public GetTransactionUseCase(ITransactionRepository repository)
        {
            _repository = repository;
        }
        public async Task<object> ExecuteAsync(Guid userId, GetTransactionDto dto)
        {
            var result = await _repository.GetByUserIdAsync(
                userId,
                dto.Page,
                dto.PageSize,
                dto.Type is null 
                    ? null 
                    : (TransactionType)dto.Type,
                dto.CategoryId,
                dto.StartDate,
                dto.EndDate
                );

            return new
            {
                dto.Page,
                dto.PageSize,
                TotalItems = result.Item2,
                Data = result.Item1
            };
        }
    }
}

namespace FinancialControl.Application.DTOs.TransactionDto
{
    public class GetTransactionQueryDto
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Type { get; set; }
    }
}

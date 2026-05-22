namespace FinancialControl.Application.DTOs.TransactionDto
{
    public class GetTransactionDto
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int? Type { get; set; }
        public Guid? CategoryId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}

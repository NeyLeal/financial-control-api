namespace FinancialControl.Application.DTOs.TransactionDto
{
    public class GetTransactionSummaryDto
    {
        public Guid? CategoryId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}

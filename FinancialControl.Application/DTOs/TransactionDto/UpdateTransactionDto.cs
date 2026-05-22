namespace FinancialControl.Application.DTOs.TransactionDto
{
    public class UpdateTransactionDto
    {
        public required Guid CategoryId { get; set; }
        public decimal Amount { get; set; }
        public required string Description { get; set; }
        public int Type { get; set; }
    }
}

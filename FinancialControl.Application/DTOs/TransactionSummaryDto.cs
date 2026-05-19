namespace FinancialControl.Application.DTOs
{
    public class TransactionSummaryDto
    {
        public decimal Income { get; set; }
        public decimal Expenses { get; set; }
        public decimal Balance { get; set; }
    }
}

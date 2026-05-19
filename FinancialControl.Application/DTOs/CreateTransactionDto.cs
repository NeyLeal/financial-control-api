namespace FinancialControl.Application.DTOs
{
    public class CreateTransactionDto
    {
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Type { get; set; }
    }
}

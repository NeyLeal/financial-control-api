namespace FinancialControl.Application.DTOs.TransactionDto
{
    public class DashboardDto
    {
        public decimal CurrentMonthIncome { get; set; }
        public decimal CurrentMonthExpenses { get; set; }
        public decimal CurrentMonthBalance { get; set; }
        public decimal Last30DaysIncome { get; set; }
        public decimal Last30DaysExpenses { get; set; }
        public List<TopCategoryDto> TopCategories { get; set; } = new();
    }
    public class TopCategoryDto
    {
        public string Category {  get; set; }
        public decimal Amount { get; set; }
    }
}

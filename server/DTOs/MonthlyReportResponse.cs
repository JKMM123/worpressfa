namespace SmeKpiDashboard.DTOs;

public class MonthlyReportResponse
{
    public int Year { get; set; }
    public int Month { get; set; }
    public decimal TotalSales { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal NetProfit { get; set; }
    public List<ExpenseByCategoryResponse> ExpensesByCategory { get; set; } = new();
}

public class ExpenseByCategoryResponse
{
    public string Category { get; set; } = string.Empty;
    public decimal Total { get; set; }
}

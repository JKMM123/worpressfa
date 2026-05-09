namespace SmeKpiDashboard.DTOs;

public class KpiSummaryResponse
{
    public decimal TotalRevenue { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal NetProfit { get; set; }
    public decimal GrowthPercentage { get; set; }
    public string? TopSellingProductName { get; set; }
    public int LowStockAlertCount { get; set; }
}

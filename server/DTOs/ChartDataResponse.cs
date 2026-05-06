namespace SmeKpiDashboard.DTOs;

public class ChartDataResponse
{
    public List<MonthlyDataPoint> MonthlyData { get; set; } = new();
}

public class MonthlyDataPoint
{
    public string Month { get; set; } = string.Empty;
    public decimal Sales { get; set; }
    public decimal Expenses { get; set; }
    public decimal Profit { get; set; }
}

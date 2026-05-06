using SmeKpiDashboard.DTOs;

namespace SmeKpiDashboard.Services;

public interface IDashboardService
{
    Task<KpiSummaryResponse> GetKpiSummaryAsync(Guid userId);
    Task<ChartDataResponse> GetChartDataAsync(Guid userId);
}

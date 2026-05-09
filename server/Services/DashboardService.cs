using SmeKpiDashboard.DTOs;
using SmeKpiDashboard.Repositories;

namespace SmeKpiDashboard.Services;

public class DashboardService : IDashboardService
{
    private readonly IDashboardRepository _repository;

    public DashboardService(IDashboardRepository repository)
    {
        _repository = repository;
    }

    public async Task<KpiSummaryResponse> GetKpiSummaryAsync(Guid userId)
    {
        return await _repository.GetKpiSummaryAsync(userId);
    }

    public async Task<ChartDataResponse> GetChartDataAsync(Guid userId)
    {
        return await _repository.GetChartDataAsync(userId);
    }

    public async Task<List<ExpenseByCategoryResponse>> GetExpenseDistributionAsync(Guid userId)
    {
        return await _repository.GetExpenseDistributionAsync(userId);
    }
}

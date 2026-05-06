using SmeKpiDashboard.DTOs;
using SmeKpiDashboard.Repositories;

namespace SmeKpiDashboard.Services;

public class ReportsService : IReportsService
{
    private readonly IDashboardRepository _repository;

    public ReportsService(IDashboardRepository repository)
    {
        _repository = repository;
    }

    public async Task<MonthlyReportResponse> GetMonthlyReportAsync(Guid userId, int year, int month)
    {
        if (month < 1 || month > 12)
            throw new ArgumentException("Month must be between 1 and 12");

        if (year < 2000 || year > 2100)
            throw new ArgumentException("Invalid year");

        return await _repository.GetMonthlyReportAsync(userId, year, month);
    }
}

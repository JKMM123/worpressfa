using SmeKpiDashboard.DTOs;

namespace SmeKpiDashboard.Services;

public interface IReportsService
{
    Task<MonthlyReportResponse> GetMonthlyReportAsync(Guid userId, int year, int month);
}

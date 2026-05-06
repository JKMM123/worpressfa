using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmeKpiDashboard.Services;

namespace SmeKpiDashboard.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _service;

    public DashboardController(IDashboardService service)
    {
        _service = service;
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst("sub")?.Value;
        return Guid.Parse(userIdClaim!);
    }

    [HttpGet("kpi-summary")]
    public async Task<IActionResult> GetKpiSummary()
    {
        var summary = await _service.GetKpiSummaryAsync(GetUserId());
        return Ok(summary);
    }

    [HttpGet("chart-data")]
    public async Task<IActionResult> GetChartData()
    {
        var chartData = await _service.GetChartDataAsync(GetUserId());
        return Ok(chartData);
    }
}

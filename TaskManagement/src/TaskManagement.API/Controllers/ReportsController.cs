using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Services;

namespace TaskManagement.API.Controllers
{
    [ApiController]
    [Route("api/reports")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("performance")]
        public async Task<ActionResult<PerformanceReportResponse>> GetPerformanceReport(
            [FromQuery] Guid userId,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            var defaultFromDate = DateTime.UtcNow.AddDays(-30);
            var defaultToDate = DateTime.UtcNow;

            var report = await _reportService.GeneratePerformanceReportAsync(
                userId,
                fromDate ?? defaultFromDate,
                toDate ?? defaultToDate);

            return Ok(report);
        }
    }
}
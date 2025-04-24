using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Services;

namespace TaskManagement.API.Controllers
{
    /// <summary>
    /// API controller for generating and retrieving various system reports
    /// </summary>
    [ApiController]
    [Route("api/reports")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        /// <summary>
        /// Initializes a new instance of the ReportsController
        /// </summary>
        /// <param name="reportService">Service for generating reports</param>
        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        /// <summary>
        /// Generates a performance report for a specific user
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET /api/reports/performance?userId=3fa85f64-5717-4562-b3fc-2c963f66afa6
        /// 
        /// Default date range is last 30 days if not specified.
        /// </remarks>
        /// <param name="userId">The ID of the user to generate the report for</param>
        /// <param name="fromDate">Start date of the reporting period (optional)</param>
        /// <param name="toDate">End date of the reporting period (optional)</param>
        /// <returns>Performance report containing metrics and statistics</returns>
        /// <response code="200">Returns the generated performance report</response>
        /// <response code="400">If the user ID is invalid or date range is invalid</response>
        /// <response code="404">If the specified user doesn't exist</response>
        [HttpGet("performance")]
        [ProducesResponseType(typeof(PerformanceReportResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
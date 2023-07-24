using ErrorOr;
using HistoryServer.Controllers;
using HistoryServer.Models;
using HistoryServer.Services.Reports;
using Microsoft.AspNetCore.Mvc;

public class ReportsController : ApiController
{

    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet]
    public IActionResult persistMessages(long timestampBegin, long timestampEnd)
    {
        ErrorOr<Report> reportResult = _reportService.GetReport(timestampBegin, timestampEnd);

        return reportResult.Match(
            report => Ok(report),
            errors => Problem(errors)
        );
    }
}

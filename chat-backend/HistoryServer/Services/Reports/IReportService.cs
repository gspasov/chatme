using HistoryServer.Models;
using ErrorOr;

namespace HistoryServer.Services.Reports;

public interface IReportService
{
    ErrorOr<Report> GetReport(long timestampBegin, long timestampEnd);
}

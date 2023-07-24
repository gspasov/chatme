using ErrorOr;
using HistoryServer.Data;
using HistoryServer.Models;

namespace HistoryServer.Services.Reports;
public class ReportService : IReportService
{
    private readonly DataContext _dataContext;
    private readonly ILogger<ReportService> _logger;

    public ReportService(DataContext dataContext, ILogger<ReportService> logger)
    {
        _dataContext = dataContext;
        _logger = logger;
    }

    public ErrorOr<Report> GetReport(long timestampBegin, long timestampEnd)
    {
        try
        {
            int totalMessages = _dataContext.Messages.Count();
            int totalUsers = _dataContext.Users.Count();
            double averageContentLength = _dataContext.Messages.Average(m => m.Content.Length);
            int maximumContentLength = _dataContext.Messages.Max(m => m.Content.Length);

            string? longestMessage = _dataContext.Messages
                .OrderByDescending(m => m.Content.Length)
                .Select(m => m.Content)
                .FirstOrDefault();

            string? shortestMessage = _dataContext.Messages
                .OrderBy(m => m.Content.Length)
                .Select(m => m.Content)
                .FirstOrDefault();

            User? mostActiveUser = _dataContext.Messages
                .GroupBy(m => m.SenderId)
                .OrderByDescending(g => g.Count())
                .Select(g => g.First().Sender)
                .FirstOrDefault();

            return new Report(
                totalMessages: totalMessages,
                totalUsers: totalUsers,
                averageContentLength: averageContentLength,
                maximumContentLength: maximumContentLength,
                longestMessage: longestMessage,
                shortestMessage: shortestMessage,
                mostActiveUser: mostActiveUser
            );
        }
        catch ( Exception ex )
        {
            _logger.LogError($"Failed to fetch report with: {ex}");

            return Error.Failure();
        }
    }
}

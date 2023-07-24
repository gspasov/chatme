namespace HistoryServer.Models;
public class Report
{ 

    public int TotalMessages { get; set; }

    public int TotalUsers { get; set; }

    public double AverageContentLength { get; set; }

    public int MaximumContentLength { get; set; }

    public string? LongestMessage { get; set; }

    public string? ShortestMessage { get; set; }

    public User? MostActiveUser { get; set; }

    public Report(
        int totalMessages, 
        int totalUsers, 
        double averageContentLength, 
        int maximumContentLength, 
        string? longestMessage, 
        string? shortestMessage, 
        User? mostActiveUser
    )
    {
        TotalMessages = totalMessages;
        TotalUsers = totalUsers;
        AverageContentLength = averageContentLength;
        MaximumContentLength = maximumContentLength;
        LongestMessage = longestMessage;
        ShortestMessage = shortestMessage;
        MostActiveUser = mostActiveUser;
    }
}

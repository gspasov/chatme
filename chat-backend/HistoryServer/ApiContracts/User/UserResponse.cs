namespace HistoryServer.ApiContracts.User;

public record UserResponse(
    string Id,
    string Username,
    bool IsOnline,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
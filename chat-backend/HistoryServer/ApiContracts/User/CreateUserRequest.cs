namespace HistoryServer.ApiContracts.User;

public record CreateUserRequest(
    string Id,
    string Username,
    bool IsOnline
);


namespace ChatServer.ApiContracts.Message;

public record CreateUserRequest(
    string Id,
    string Username,
    bool IsOnline
);

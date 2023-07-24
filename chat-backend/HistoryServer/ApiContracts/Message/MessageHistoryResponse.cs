namespace HistoryServer.ApiContracts.Message;
public record MessageHistoryResponse(
    string SenderId,
    string SenderUsername,
    string Content,
    DateTime CreatedAt
);

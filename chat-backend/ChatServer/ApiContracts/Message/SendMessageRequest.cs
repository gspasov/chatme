namespace ChatServer.ApiContracts.Message;
public record SendMessageRequest(
    string Id,
    string Content,
    string SenderId,
    string ReceiverId,
    long Timestamp
);
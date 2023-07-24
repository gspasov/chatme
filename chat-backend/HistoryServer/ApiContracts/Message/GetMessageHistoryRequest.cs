namespace HistoryServer.ApiContracts.Message;
public record GetMessageHistoryRequest(
    int senderId,
    int receiverId
);

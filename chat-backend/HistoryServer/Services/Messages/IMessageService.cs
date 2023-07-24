using HistoryServer.ApiContracts.Message;
using HistoryServer.Models;
using ErrorOr;

namespace HistoryServer.Services.Messages;
public interface IMessageService
{
    ErrorOr<List<Message>> FetchMessageHistory(string senderId, string receiverId);

    Task<ErrorOr<Created>> PersistMessageHistory(IDictionary<string, List<SendMessageRequest>> history);
}

using ChatServer.Models;

namespace ChatServer.Services.Storage;
public interface IStorageService
{
    Task<List<User>> GetUsers();

    Task<HttpResponseMessage> CreateUser(User user);

    Task<HttpResponseMessage> DisconnectUser(string userId);

    Task<HttpResponseMessage> PersistMessages(IDictionary<string, List<Message>> messages);


}

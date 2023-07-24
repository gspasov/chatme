using ChatServer.Models;

namespace ChatServer.Services.Storage;
public class StorageService : IStorageService
{
    private readonly HttpClient _httpClient;
    private static readonly string BaseApiUrl = "https://localhost:7030/api";

    public StorageService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<HttpResponseMessage> CreateUser(User user)
    {
        return await _httpClient.PostAsJsonAsync($"{BaseApiUrl}/users", user);
    }

    public Task<HttpResponseMessage> DisconnectUser(string userId)
    {
        return _httpClient.GetAsync($"{BaseApiUrl}/users/{userId}/disconnect");
    }

    public async Task<List<User>> GetUsers()
    {
        List<User>? users = await _httpClient.GetFromJsonAsync<List<User>>($"{BaseApiUrl}/users");
        if (users == null)
        {
            return new List<User>();
        }

        return users;
    }

    public async Task<HttpResponseMessage> PersistMessages(IDictionary<string, List<Message>> messages)
    {
        return await _httpClient.PostAsJsonAsync($"{BaseApiUrl}/messages/history", messages);
    }
}


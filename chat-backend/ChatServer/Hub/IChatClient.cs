using ChatServer.ApiContracts.Message;
using ChatServer.Models;

namespace ChatServer.Hub;

public interface IChatClient
{
    Task ReceiveMessage(SendMessageRequest message);

    Task EstablishConnection(EstablishConnection connection);

    Task UserConnect(User user);

    Task UserDisconnect(string userId);

    Task ReceiveMessageS(string message);
}
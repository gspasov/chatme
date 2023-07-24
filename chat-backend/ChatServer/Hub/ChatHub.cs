using ChatServer.ApiContracts.Message;
using ChatServer.Models;
using ChatServer.Services.Storage;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace ChatServer.Hub;
public sealed class ChatHub : Hub<IChatClient>
{
    private readonly IStorageService _storageService;
    private readonly IDictionary<string, List<Message>> _messages;

    public ChatHub(
        IStorageService storageService,
        IDictionary<string, List<Message>> messages
    )
    {
        _messages = messages;
        _storageService = storageService;
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);

        await _storageService.DisconnectUser(Context.ConnectionId);
        await Clients.Others.UserDisconnect(Context.ConnectionId);
    }

    public async Task Connect(string username)
    {
        var user = new User(id: Context.ConnectionId, username: username);

        List<User> currentSystemUsers = await _storageService.GetUsers();
        await _storageService.CreateUser(user);

        var connection = new EstablishConnection(connectionId: Context.ConnectionId, users: currentSystemUsers);

        // Send all users to the Connector
        await Clients.Caller.EstablishConnection(connection);

        // Broadcast new connected user to already connected users
        await Clients.Others.UserConnect(user);
    }

    public async Task SendMessage(SendMessageRequest messageRequest)
    {
        var message = new Message(messageRequest);

        string converesationId = BuildConversationId(message.SenderId, message.ReceiverId);
        if (_messages.ContainsKey(converesationId))
        {
            _messages[converesationId].Add(message);
        }
        else
        {
            // Create a Chat Room for the Sender and Receiver
            await Groups.AddToGroupAsync(message.SenderId, converesationId);
            await Groups.AddToGroupAsync(message.ReceiverId, converesationId);

            _messages[converesationId] = new List<Message> { message };
        }

        // Send a message to the Chat Room
        await Clients.GroupExcept(converesationId, Context.ConnectionId).ReceiveMessage(messageRequest);
    }

    private static string BuildConversationId(string senderId, string receiverId)
    {
        string[] ids = new string[] { senderId, receiverId };

        Array.Sort(ids);

        return string.Join("#", ids);
    }
}

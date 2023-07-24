using ChatServer.ApiContracts.Message;

namespace ChatServer.Models;
public class Message
{
    public string Id { get; set; }

    public string Content { get; set; }

    public string SenderId { get; set; }

    public string ReceiverId { get; set; }

    public long Timestamp { get; set; }

    public bool IsPersisted { get; set; } = false;

    public Message(SendMessageRequest request)
    {
        Id = request.Id;
        Content = request.Content;
        SenderId = request.SenderId;
        ReceiverId = request.ReceiverId;
        Timestamp = request.Timestamp;
    }

    public Message(string id, string content, User sender, User receiver, long timestamp, bool isPersisted)
    {
        Id = id;
        Content = content;
        SenderId = sender.Id;
        ReceiverId = receiver.Id;
        Timestamp = timestamp;
        IsPersisted = isPersisted;
    }
}

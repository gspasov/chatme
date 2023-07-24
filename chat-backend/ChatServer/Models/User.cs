using ChatServer.ApiContracts.Message;
using System.Text.Json.Serialization;

namespace ChatServer.Models;
public class User
{
    public string Id { get; set; }

    public string Username { get; set; } = "";

    public bool IsOnline { get; set; } = true;

    [JsonConstructor]
    public User(string id, string username, bool isOnline)
    {
        Id = id;
        Username = username;
        IsOnline = isOnline;
    }

    public User(string id, string username)
    {
        Id = id;
        Username = username;
    }

    public User Disconnect()
    {
        IsOnline = false;

        return this;
    }

}
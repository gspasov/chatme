namespace ChatServer.Models;
public class EstablishConnection
{
    public string ConnectionId { get; set; }
    public List<User> Users { get; set; }

    public EstablishConnection(string connectionId, List<User> users)
    {
        ConnectionId = connectionId;
        Users = users;
    }
}

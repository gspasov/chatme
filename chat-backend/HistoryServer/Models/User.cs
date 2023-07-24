using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HistoryServer.Models;

[Table("users")]
public class User {

    [Key]
    [Column("id")]
    public string Id { get; set;  }

    [Required]
    [Column("username")]
    public string Username { get; set; } = "";

    [Required]
    [Column("isOnline")]
    public bool IsOnline { get; set; } = true;

    [Required]
    [Column("createdAt")]
    public DateTime CreatedAt { get; set; }

    [Required]
    [Column("updatedAt")]
    public DateTime UpdatedAt { get; set; }


    public User(string id)
    {
        DateTime now = DateTime.UtcNow;
        Id = id;
        CreatedAt = now;
        UpdatedAt = now;
    }

    public User(string id, string username)
    {
        DateTime now = DateTime.UtcNow;
        Id = id;
        Username = username;
        CreatedAt = now;
        UpdatedAt = now;
    }

    public User(string id, string username, bool isOnline)
    {
        DateTime now = DateTime.UtcNow;
        Id = id;
        Username = username;
        IsOnline = isOnline;
        CreatedAt = now;
        UpdatedAt = now;
    }

    //public User(string username, string firstName, string lastName, string connectionId)
    //{
    //    Username = username;
    //    FirstName = firstName;
    //    LastName = lastName;
    //    ConnectionId = connectionId;
    //}
}


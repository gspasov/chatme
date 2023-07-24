using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HistoryServer.Models;


[Table("messages")]
public class Message
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("content")]
    public string Content { get; set; } = "";

    [Column("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("conversationId")]
    public int ConversationId { get; set; }

    [Column("senderId")]
    public string SenderId { get; set; }

    [Column("sender")]
    public virtual User? Sender { get; set; }

    [Column("conversation")]
    public virtual Conversation? Conversation { get; set; }

    public Message(string content, string senderId)
    {
        Content = content;
        SenderId = senderId;
    }

    public Message(string content, string senderId, Conversation conversation)
    {
        Content = content;
        SenderId = senderId;
        Conversation = conversation;
    }
}


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HistoryServer.Models;

[Table("conversations")]
public class Conversation
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("conversaionId")]
    public string ConversationId { get; set; } = "";

    [Column("personAId")]
    public string PersonAId { get; set; }

    [Column("personBId")]
    public string PersonBId { get; set; }

    [Column("personA")]
    public virtual User? PersonA { get; set; }

    [Column("personB")]
    public virtual User? PersonB { get; set; }

    public virtual ICollection<Message> Messages { get; } = new List<Message>();

    public Conversation(string conversationId, string personAId, string personBId)
    {
        ConversationId = conversationId;
        PersonAId = personAId;
        PersonBId = personBId;
    }
}

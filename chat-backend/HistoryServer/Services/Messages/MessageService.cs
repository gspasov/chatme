using HistoryServer.Data;
using HistoryServer.Models;
using HistoryServer.ServiceErrors;
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using HistoryServer.ApiContracts.Message;

namespace HistoryServer.Services.Messages;
public class MessageService : IMessageService
{
    private readonly DataContext _dataContext;

    public MessageService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public ErrorOr<List<Message>> FetchMessageHistory(string senderId, string receiverId)
    {
        Conversation? conversation = _dataContext.Conversations
            .Include(c => c.Messages)
            .ThenInclude(m => m.Sender)
            .FirstOrDefault(c =>
                (c.PersonAId == senderId && c.PersonBId == receiverId)
                || (c.PersonAId == receiverId && c.PersonBId == senderId)
            );

        if (conversation == null)
        {
            return Errors.Conversation.NotFound;
        }
        return conversation.Messages.ToList();
    }
    public async Task<ErrorOr<Created>> PersistMessageHistory(IDictionary<string, List<SendMessageRequest>> history)
    {
        foreach (KeyValuePair<string, List<SendMessageRequest>> kvp in history)
        {
            string conversationId = kvp.Key;
            SendMessageRequest smr = kvp.Value.First();

            Conversation conversation = 
                FindConversation(conversationId) 
                ?? CreateConversation(
                    conversationId: conversationId, 
                    personAId: smr.SenderId, 
                    personBId: smr.ReceiverId
                );

            foreach(SendMessageRequest messageRequest in kvp.Value)
            {
                var message = new Message(
                    content: messageRequest.Content,
                    senderId: messageRequest.SenderId,
                    conversation: conversation
                );

                _dataContext.Messages.Add(message);
            }
        }

        await _dataContext.SaveChangesAsync();

        return Result.Created;
    }

    private Conversation? FindConversation(string conversationId)
    {
        return _dataContext.Conversations.FirstOrDefault(c =>
            c.ConversationId == conversationId
        );
    }

    private Conversation? FindConversation(string senderId, string receiverId)
    {
        return _dataContext.Conversations.FirstOrDefault(c =>
            (c.PersonAId == senderId && c.PersonBId == receiverId)
            || (c.PersonAId == receiverId && c.PersonBId == senderId)
        );
    }

    private Conversation CreateConversation(string conversationId, string personAId, string personBId)
    {
        var conversation = new Conversation(
            conversationId: conversationId, 
            personAId: personAId, 
            personBId: personBId
        );

        _dataContext.Conversations.Add(conversation);

        return conversation;
    }
}

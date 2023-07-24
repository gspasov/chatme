using HistoryServer.ApiContracts.Message;
using HistoryServer.Models;
using HistoryServer.Services.Messages;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace HistoryServer.Controllers;
public class MessagesController : ApiController
{
    private IMessageService _messageService;

    public MessagesController(IMessageService messageService)
    {
        _messageService = messageService;
    }

    [HttpPost("history")]
    public async Task<IActionResult> persistMessages(Dictionary<string, List<SendMessageRequest>> request)
    {
        ErrorOr<Created> persistHistory = await _messageService.PersistMessageHistory(request);

        return persistHistory.Match(
            created => NoContent(),
            errors => Problem(errors)
        );
    }

    [HttpGet]
    public IActionResult getMessageHistory(string senderId, string receiverId)
    {
        ErrorOr<List<Message>> historyResult = _messageService.FetchMessageHistory(
            senderId: senderId,
            receiverId: receiverId
        );

        return historyResult.Match(
            messages => Ok(messages.Select(m => MapHistoryResponse(m))),
            errors => Problem(errors)
        );
    }

    private static MessageHistoryResponse MapHistoryResponse(Message message)
    {
        return new MessageHistoryResponse(
            SenderId: message.SenderId,
            SenderUsername: message.Sender?.Username ?? "missing_username",
            Content: message.Content,
            CreatedAt: message.CreatedAt
        );
    }
}

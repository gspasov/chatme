using ChatServer.Models;
using System.Text.Json;

namespace ChatServer.Services.Storage;
public class PersistMessageHistoryService : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly IStorageService _storageService;
    private readonly IDictionary<string, List<Message>> _messages;
    private readonly ILogger<PersistMessageHistoryService> _logger;

    public PersistMessageHistoryService(
        IConfiguration configuration,
        IStorageService storageService,
        IDictionary<string, List<Message>> messages, 
        ILogger<PersistMessageHistoryService> logger
    )
    {
        _logger = logger;
        _messages = messages;
        _configuration = configuration;
        _storageService = storageService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        int minutes = _configuration.GetValue<int>("PersistantPeriod:Minutes");
        PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromMinutes(minutes));

        while (await timer.WaitForNextTickAsync(stoppingToken)
            && !stoppingToken.IsCancellationRequested)
        {
            IDictionary<string, List<Message>> notPersistedMessages = _messages
                .Where(entry => entry.Value.Count > 0 && entry.Value.Any(message => !message.IsPersisted))
                .ToDictionary(
                    entry => entry.Key, 
                    entry => entry.Value.Where(message => !message.IsPersisted).ToList()
                );

            if (notPersistedMessages.Values.Any(messages => messages.Count > 0))
            {
                try
                {
                    HttpResponseMessage response = await _storageService.PersistMessages(notPersistedMessages);
                    if (response.IsSuccessStatusCode)
                    {
                        // Mark all messages as persisted
                        _messages.Values
                            .SelectMany(messages => messages)
                            .ToList()
                            .ForEach(message => message.IsPersisted = true);
                    }
                } 
                catch (Exception ex)
                {
                    _logger.LogError($"Persisting messages failed with {ex}");
                }
            }
            else
            {
                _logger.LogInformation("No new messages to persist..");
            }
            
        }
    }
}

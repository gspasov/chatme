using ChatServer.Hub;
using ChatServer.Models;
using ChatServer.Services.Storage;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddSignalR();

    builder.Services.AddSingleton<IStorageService, StorageService>();
    builder.Services.AddSingleton<IDictionary<string, User>>(opts => new Dictionary<string, User>());
    builder.Services.AddSingleton<IDictionary<string, List<Message>>>(opts => new Dictionary<string, List<Message>>());

    builder.Services.AddHttpClient<StorageService>();
    builder.Services.AddHostedService<PersistMessageHistoryService>();

    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(builder =>
        {
            builder.WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();

        });
    });
}

var app = builder.Build();
{
    app.UseRouting();
    app.UseCors();

    app.UseEndpoints(endpoints =>
    {
        app.MapHub<ChatHub>("chat-hub");
    });

    app.Run();
}


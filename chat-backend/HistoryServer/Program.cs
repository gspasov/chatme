using Microsoft.EntityFrameworkCore;
using HistoryServer.Data;
using HistoryServer.Services.Users;
using HistoryServer.Services.Messages;
using HistoryServer.Services.Reports;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();

    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IReportService, ReportService>();
    builder.Services.AddScoped<IMessageService, MessageService>();

    builder.Services.AddDbContext<DataContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection"))
    );

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseExceptionHandler("/error");
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}

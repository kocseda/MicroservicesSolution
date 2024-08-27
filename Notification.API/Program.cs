using MediatR;
using Microsoft.Extensions.Options;
using Notification.Application.Handlers;
using Notification.Application.Services;
using Notification.Infrastructure.Email;
using Notification.Infrastructure.Messaging;
using System.Net;
using System.Net.Mail;

var builder = WebApplication.CreateBuilder(args);

// Register SmtpSettings from the configuration
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

// Register SmtpClient
builder.Services.AddTransient<SmtpClient>(sp =>
{
    var smtpSettings = sp.GetRequiredService<IOptions<SmtpSettings>>().Value;

    return new SmtpClient
    {
        Host = smtpSettings.Host,
        Port = smtpSettings.Port,
        EnableSsl = smtpSettings.EnableSsl,
        Credentials = new NetworkCredential(smtpSettings.UserName, smtpSettings.Password)
    };
});

builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<INotificationHandler<StockUpdatedNotification>, StockUpdatedHandler>();
builder.Services.AddMediatR(typeof(StockUpdatedHandler).Assembly);
builder.Services.AddSingleton<MessageConsumer>();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var notificationConsumerService = app.Services.GetRequiredService<MessageConsumer>();
notificationConsumerService.StartConsuming();

app.Run();


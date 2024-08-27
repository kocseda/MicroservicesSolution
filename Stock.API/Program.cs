using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Messages.Events;
using Stock.Application.Handlers;
using Stock.Application.Interfaces;
using Stock.Application.Services;
using Stock.Domain.Data;
using Stock.Domain.Repositories;
using Stock.Infrastructure.Messaging;
using Stock.Infrastructure.Repositories;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<StockContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<INotificationHandler<OrderPlacedNotification>, OrderPlacedHandler>();
builder.Services.AddMediatR(typeof(OrderPlacedHandler).Assembly);

builder.Services.AddScoped<IMessagePublisher, MessagePublisher>();
builder.Services.AddSingleton<MessageConsumer>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();



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

var stockConsumerService = app.Services.GetRequiredService<MessageConsumer>();
stockConsumerService.StartConsuming();

app.Run();


using MediatR;
using Notification.Domain.Entities;
using System.Net.Mail;
using Shared.Messages.Events;
using Notification.Application.Models;
using Notification.Application.Services;

namespace Notification.Application.Handlers
{
    public class StockUpdatedNotification : INotification
    {
        public StockUpdated StockUpdated { get; set; }
    }
    public class StockUpdatedHandler : INotificationHandler<StockUpdatedNotification>
    {
        private readonly IEmailSender _emailSender;

        public StockUpdatedHandler(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public async Task Handle(StockUpdatedNotification notification, CancellationToken cancellationToken)
        {
            var stockUpdated = notification.StockUpdated;
            var subject = "About " + stockUpdated.OrderId + " No Order";
            var body = stockUpdated.IsStockAvailable ? "The Order with ID " + stockUpdated.OrderId + " has been placed successfully!" : "The Order with ID " + stockUpdated.OrderId + " has been placed but the following products are out of stock: " + string.Join(", ", stockUpdated.Products);
            var message = new EmailMessage(stockUpdated.CustomerContact, subject, body);
            await _emailSender.SendEmailAsync(message);
        }
    }
}

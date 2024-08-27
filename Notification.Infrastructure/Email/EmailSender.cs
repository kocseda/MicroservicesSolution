using System.Net.Mail;
using static Notification.Infrastructure.Email.EmailSender;
using System.Net;
using Microsoft.Extensions.Configuration;
using Notification.Application.Services;
using Notification.Application.Models;

namespace Notification.Infrastructure.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly SmtpClient _smtpClient;
        private readonly IConfiguration _configuration;

        public EmailSender(SmtpClient smtpClient, IConfiguration configuration)
        {
            _smtpClient = smtpClient;
            _configuration = configuration;
        }
        public async Task SendEmailAsync(EmailMessage emailMessage)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                              | SecurityProtocolType.Tls11
                                              | SecurityProtocolType.Tls12;

            var from = _configuration.GetValue<string>("SmtpSettings:UserName");

            var mailMessage = new MailMessage(from, emailMessage.To)
            {
                Subject = emailMessage.Subject,
                Body = emailMessage.Body,
                IsBodyHtml = false
            };
            _smtpClient.UseDefaultCredentials = false;
            _smtpClient.ServicePoint.MaxIdleTime = 1;
            _smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            _smtpClient.TargetName = "STARTTLS/smtp.office365.com";
            await _smtpClient.SendMailAsync(mailMessage);
        }

    }
}
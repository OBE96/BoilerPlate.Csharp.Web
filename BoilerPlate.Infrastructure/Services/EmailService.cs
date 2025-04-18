using BoilerPlate.Domain.Entities;
using BoilerPlate.Infrastructure.Services.Interfaces;
using BoilerPlate.Infrastructure.Utilities;
using BoilerPlate.Infrastructure.Utilities.StringKeys;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using MimeKit;
using System.Text.Json;



namespace BoilerPlate.Infrastructure.Services
{
    internal class EmailService(SmtpCredentials smtpCredentials, ILogger<EmailService> logger) : IEmailService
    {
        private readonly SmtpCredentials smtpDetails = smtpCredentials;
        private readonly ILogger<EmailService> logger = logger;
        public async Task<Message> SendEmailMessage(Message message)
        {
            logger.LogDebug("Sending the passed email message from the email service");

            logger.LogDebug($"SMTP CREDENTIALS:\n{JsonSerializer.Serialize(smtpDetails)}");

            MimeMessage emailMessage = new();
            emailMessage.From.Add(new MailboxAddress(EmailConstants.senderMailboxName, EmailConstants.senderMailboxAddress));
            emailMessage.To.Add(new MailboxAddress($"{message.RecipientName}", $"{message.RecipientContact}"));
            emailMessage.Subject = message.Subject;

            var bodyBuilder = new BodyBuilder();
            // Check if the message content is HTML
            if (message.IsHtml) // assuming the Message class now has the IsHtml property
            {
                bodyBuilder.HtmlBody = message.Content; // set HTML content
            }
            else
            {
                bodyBuilder.TextBody = message.Content; // set plain text content
            }

            emailMessage.Body = bodyBuilder.ToMessageBody();

            //emailMessage.Body = new BodyBuilder() { TextBody = message.Content }.ToMessageBody();

            using var client = new SmtpClient();

            await client.ConnectAsync(smtpDetails.Host, smtpDetails.Port, SecureSocketOptions.Auto);
            await client.AuthenticateAsync(smtpDetails.Username, smtpDetails.Password);
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);

            return message;

        }
    }
}
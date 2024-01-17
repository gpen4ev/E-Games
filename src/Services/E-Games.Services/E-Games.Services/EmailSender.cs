using E_Games.Services.E_Games.Services.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;

namespace E_Games.Services.E_Games.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailSender(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail));
            mimeMessage.To.Add(new MailboxAddress("John Doe", email));
            mimeMessage.Subject = subject;

            mimeMessage.Body = new TextPart("html") { Text = message };

            using (var smtpClient = new MailKit.Net.Smtp.SmtpClient())
            {
                await smtpClient.ConnectAsync(_smtpSettings.Server, _smtpSettings.Port, _smtpSettings.UseSsl);
                await smtpClient.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
                await smtpClient.SendAsync(mimeMessage);
                await smtpClient.DisconnectAsync(true);
            }
        }
    }
}

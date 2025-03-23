using MailKit.Net.Smtp;
using MimeKit;
using Tapir.Core.Mailing;

namespace Tapir.Providers.Mailing.MailKit.Client
{
    public class MailClient : IMailClient
    {
        private readonly ISmtpClient _smtpClient;
        private readonly Configuration _configuration;

        public MailClient(ISmtpClient smtpClient, Configuration configuration)
        {
            _smtpClient = smtpClient;
            _configuration = configuration;
        }

        public async Task Send(MailMessage message)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress(_configuration.From, _configuration.From));
            mimeMessage.To.Add(new MailboxAddress(message.To, message.To));
            mimeMessage.Subject = message.Subject;
            mimeMessage.Body = new TextPart("html")
            {
                Text = message.Body
            };

            await _smtpClient.ConnectAsync(_configuration.Host, _configuration.Port, _configuration.UseSsl);
            await _smtpClient.AuthenticateAsync(_configuration.Username, _configuration.Password);
            await _smtpClient.SendAsync(mimeMessage);
            await _smtpClient.DisconnectAsync(true);
        }
    }
}

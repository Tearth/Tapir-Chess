using Microsoft.Extensions.Configuration;
using MimeKit.Text;
using System.Reflection;
using System.Text;
using Tapir.Core.Mailing;
using Tapir.Core.Scheduler;

namespace Tapir.Identity.Application.Auth.Mails.EmailConfirmation
{
    public class EmailConfirmationMailTask : MailBase, ITask
    {
        public string To { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }

        private readonly IMailClient _mailClient;
        private readonly IConfiguration _configuration;

        public EmailConfirmationMailTask()
        {

        }

        public EmailConfirmationMailTask(IMailClient mailClient, IConfiguration configuration)
        {
            _mailClient = mailClient;
            _configuration = configuration;
        }

        public async Task Run()
        {
            var content = await ReadTemplate("EmailConfirmationMailTemplate.html");
            var urlTemplate = _configuration["Endpoints:EmailConfirmation"];
            
            urlTemplate = urlTemplate.Replace("{USERID}", Convert.ToBase64String(Encoding.UTF8.GetBytes(UserId)));
            urlTemplate = urlTemplate.Replace("{TOKEN}", Convert.ToBase64String(Encoding.UTF8.GetBytes(Token)));
            content = content.Replace("{URL}", urlTemplate);

            await _mailClient.Send(new MailMessage
            {
                To = To,
                Subject = "Confirm your account",
                Body = content
            });
        }
    }
}

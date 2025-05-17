using System.Text;
using Tapir.Core.Mailing;
using Tapir.Core.Scheduler;
using Tapir.Identity.Infrastructure;

namespace Tapir.Identity.Application.Auth.Mails.PasswordReset
{
    public class PasswordResetMailTask : MailBase, ITask
    {
        public required string To { get; set; }
        public required Guid UserId { get; set; }
        public required string Token { get; set; }

        private readonly IMailClient _mailClient;
        private readonly AppSettings _settings;

        public PasswordResetMailTask(IMailClient? mailClient = null, AppSettings? settings = null)
        {
            _mailClient = mailClient!;
            _settings = settings!;
        }

        public async Task Run()
        {
            var content = await ReadTemplate("PasswordResetMailTemplate.html");
            var urlTemplate = _settings.Endpoints.PasswordReset;
            
            urlTemplate = urlTemplate.Replace("{USERID}", Convert.ToBase64String(Encoding.UTF8.GetBytes(UserId.ToString())));
            urlTemplate = urlTemplate.Replace("{TOKEN}", Convert.ToBase64String(Encoding.UTF8.GetBytes(Token)));
            content = content.Replace("{URL}", urlTemplate);

            await _mailClient.Send(new MailMessage
            {
                To = To,
                Subject = "Reset your password",
                Body = content
            });
        }
    }
}

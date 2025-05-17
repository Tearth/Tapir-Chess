using System.Text;
using Tapir.Core.Mailing;
using Tapir.Core.Scheduler;
using Tapir.Identity.Infrastructure;

namespace Tapir.Identity.Application.Auth.Mails.EmailChange
{
    public class EmailChangeMailTask : MailBase, ITask
    {
        public required string To { get; set; }
        public required Guid UserId { get; set; }
        public required string Token { get; set; }
        public required string NewEmail { get; set; }

        private readonly IMailClient _mailClient;
        private readonly AppSettings _settings;

        public EmailChangeMailTask(IMailClient? mailClient = null, AppSettings? settings = null)
        {
            _mailClient = mailClient!;
            _settings = settings!;
        }

        public async Task Run()
        {
            var content = await ReadTemplate("EmailChangeMailTemplate.html");
            var urlTemplate = _settings.Endpoints.EmailChange;
            
            urlTemplate = urlTemplate.Replace("{USERID}", Convert.ToBase64String(Encoding.UTF8.GetBytes(UserId.ToString())));
            urlTemplate = urlTemplate.Replace("{TOKEN}", Convert.ToBase64String(Encoding.UTF8.GetBytes(Token)));
            urlTemplate = urlTemplate.Replace("{EMAIL}", Convert.ToBase64String(Encoding.UTF8.GetBytes(NewEmail)));
            content = content.Replace("{URL}", urlTemplate);

            await _mailClient.Send(new MailMessage
            {
                To = To,
                Subject = "Change your e-mail",
                Body = content
            });
        }
    }
}

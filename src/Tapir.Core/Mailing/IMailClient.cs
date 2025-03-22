namespace Tapir.Core.Mailing
{
    public interface IMailClient
    {
        Task Send(MailMessage message);
    }
}

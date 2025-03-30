namespace Tapir.Core.Mailing
{
    public class MailMessage
    {
        public required string To { get; set; }
        public required string Subject { get; set; }
        public required string Body { get; set; }
    }
}

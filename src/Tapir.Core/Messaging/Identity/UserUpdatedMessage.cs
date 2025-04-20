namespace Tapir.Core.Messaging.Identity
{
    public class UserUpdatedMessage
    {
        public required Guid Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
    }
}

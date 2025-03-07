namespace Tapir.Core.Exceptions
{
    public class EventNotRegisteredException : Exception
    {
        public EventNotRegisteredException()
        {

        }

        public EventNotRegisteredException(string? message) : base(message)
        {

        }

        public EventNotRegisteredException(string? message, Exception? innerException) : base(message, innerException)
        {

        }
    }
}

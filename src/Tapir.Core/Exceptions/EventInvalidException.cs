namespace Tapir.Core.Exceptions
{
    public class EventInvalidException : Exception
    {
        public EventInvalidException()
        {

        }

        public EventInvalidException(string? message) : base(message)
        {

        }

        public EventInvalidException(string? message, Exception? innerException) : base(message, innerException)
        {

        }
    }
}

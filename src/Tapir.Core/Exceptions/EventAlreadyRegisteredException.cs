namespace Tapir.Core.Exceptions
{
    public class EventAlreadyRegisteredException : Exception
    {
        public EventAlreadyRegisteredException()
        {

        }

        public EventAlreadyRegisteredException(string? message) : base(message)
        {

        }

        public EventAlreadyRegisteredException(string? message, Exception? innerException) : base(message, innerException)
        {

        }
    }
}

namespace Tapir.Core.Persistence.Exceptions
{
    public class AggregateConcurrentWriteException : Exception
    {
        public AggregateConcurrentWriteException()
        {

        }

        public AggregateConcurrentWriteException(string? message) : base(message)
        {

        }

        public AggregateConcurrentWriteException(string? message, Exception? innerException) : base(message, innerException)
        {

        }
    }
}

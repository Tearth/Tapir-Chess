namespace Tapir.Core.Domain.Exceptions
{
    public class DomainException : Exception
    {
        public string? ErrorCode { get; set; }

        public DomainException()
        {

        }

        public DomainException(string? message, string? errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }

        public DomainException(string? message, string? errorCode, Exception? innerException) : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }
}

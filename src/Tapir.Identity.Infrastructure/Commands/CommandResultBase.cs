namespace Tapir.Identity.Infrastructure.Commands
{
    public class CommandResultBase<T> where T: CommandResultBase<T>, new()
    {
        public bool Success { get; set; }
        public string? ErrorCode { get; set; }

        public static T Error(string errorCode)
        {
            return new T
            {
                Success = false,
                ErrorCode = errorCode
            };
        }
    }
}

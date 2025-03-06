namespace Tapir.Core.Queries
{
    public class QueryResult<T>
    {
        public T Value { get; set; }
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }

        public static QueryResult<T> Success(T value)
        {
            return new QueryResult<T>
            {
                Value = value,
                IsValid = true
            };
        }

        public static QueryResult<T> Error(string message)
        {
            return new QueryResult<T>
            {
                ErrorMessage = message
            };
        }
    }
}

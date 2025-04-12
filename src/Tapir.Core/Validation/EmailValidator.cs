using System.Text.RegularExpressions;

namespace Tapir.Core.Validation
{
    public static partial class EmailValidator
    {
        [GeneratedRegex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")]
        private static partial Regex EmailRegex();

        public static bool IsValid(string email)
        {
            return EmailRegex().IsMatch(email);
        }
    }
}

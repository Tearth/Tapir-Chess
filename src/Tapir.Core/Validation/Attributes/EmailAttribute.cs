using System.ComponentModel.DataAnnotations;

namespace Tapir.Core.Validation.Attributes
{
    public class EmailAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return true;
            }

            if (value is not string email)
            {
                return false;
            }

            return EmailValidator.IsValid(email);
        }
    }
}

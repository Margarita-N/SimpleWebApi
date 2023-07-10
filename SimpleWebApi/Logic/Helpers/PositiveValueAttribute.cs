using System.ComponentModel.DataAnnotations;

namespace SimpleWebApi.Logic.Helpers
{
    public class PositiveValueAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            var parsedValue = (decimal)value;

            if (parsedValue > 0)
                return true;

            return false;
        }
    }
}

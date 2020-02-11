using System;
using System.ComponentModel.DataAnnotations;

namespace RS1_Ispit_asp.net_core.Helpers
{
    public class FutureDateTime : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            ErrorMessage = "Datum mora biti u buducnosti.";
            if (value is DateTime)
            {
                DateTime.TryParse(value.ToString(), out var date);

                if (date <= DateTime.Now)
                {
                    return new ValidationResult(FormatErrorMessage(context.DisplayName));

                }
                return ValidationResult.Success;
            }

            return ValidationResult.Success;
        }
    }
}
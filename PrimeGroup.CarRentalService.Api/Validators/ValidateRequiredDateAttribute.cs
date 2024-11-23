using System.ComponentModel.DataAnnotations;

public class ValidateRequiredDateAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is DateTime date && date == DateTime.MinValue)
        {
            string propertyName = validationContext.DisplayName; // Get the name of the property
            return new ValidationResult($"{propertyName} is required.");
        }

        return ValidationResult.Success;
    }
}

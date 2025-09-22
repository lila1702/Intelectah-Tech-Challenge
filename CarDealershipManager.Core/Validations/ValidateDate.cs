using System.ComponentModel.DataAnnotations;

namespace CarDealershipManager.Core.Validations
{
    public class ValidateDate : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime date)
            {
                if (date > DateTime.UtcNow.Date)
                {
                    return new ValidationResult(ErrorMessage ?? "A data não pode ser futura.");
                }
                return ValidationResult.Success;
            }

            return new ValidationResult("Data inválida.");
        }
    }
}

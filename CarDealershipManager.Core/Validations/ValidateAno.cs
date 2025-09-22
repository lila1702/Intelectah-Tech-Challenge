using System;
using System.ComponentModel.DataAnnotations;

namespace CarDealershipManager.Core.Validations
{
    public class ValidateAno : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is int ano)
            {
                int anoAtual = DateTime.Now.Year;
                if (ano >= 1800 && ano <= anoAtual)
                    return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage ?? "Ano inválido");
        }
    }
}

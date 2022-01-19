using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Models.CustomValidations
{
    public class FirstCaseUpperAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
           if(value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            var firstCase = value.ToString()[0].ToString();
            if (firstCase != firstCase.ToUpper())
            {
                return new ValidationResult("La primera letra debe ser Mayuscula");
            }

            return ValidationResult.Success;
        }
    }
}

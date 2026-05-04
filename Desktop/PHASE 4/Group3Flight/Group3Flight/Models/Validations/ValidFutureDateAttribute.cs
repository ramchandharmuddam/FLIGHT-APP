using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Group3Flight.Models.Validations
{
    public class ValidFutureDateAttribute : ValidationAttribute, IClientModelValidator
    {
        private readonly int _maxYears;

        public ValidFutureDateAttribute(int years)
        {
            _maxYears = years;
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime inputDate)
            {
                var today = DateTime.Today;
                var limitDate = today.AddYears(_maxYears);

                if (inputDate > today && inputDate <= limitDate)
                {
                    return ValidationResult.Success!;
                }
            }

            return new ValidationResult(GetErrorMessage(
                validationContext.DisplayName ?? "Date"));
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            if (!context.Attributes.ContainsKey("data-val"))
            {
                context.Attributes.Add("data-val", "true");
            }

            context.Attributes.Add("data-val-validdate-years", _maxYears.ToString());

            context.Attributes.Add(
                "data-val-validdate",
                GetErrorMessage(
                    context.ModelMetadata.DisplayName ??
                    context.ModelMetadata.Name ??
                    "Date"));
        }

        private string GetErrorMessage(string fieldName)
        {
            return ErrorMessage ??
                   $"{fieldName} must be a valid future date within {_maxYears} years.";
        }
    }
}
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BookingBlock.WebApplication.Models.ValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property |
                    AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class BusinessTypeCodeValidatorAttribute : ValidationAttribute
    {
        private class SimpleValidationResult
        {
            public bool IsValid { get; set; }

            public string Message { get; set; }
        }

        private SimpleValidationResult VaidateCode(string code)
        {
            if (!string.IsNullOrWhiteSpace(code))
            {
                if (code.Length == 3)
                {
                    if (Regex.IsMatch(code, @"^[a-zA-Z]+$"))
                    {
                        return new SimpleValidationResult() { IsValid = true };    
                    }

                    return new SimpleValidationResult() { Message = "Business type code must only contain letters." };

                }

                return new SimpleValidationResult() { Message = "Business type code must be exactly 3 characters."};
            }

            return new SimpleValidationResult() { IsValid = false, Message = "You must specify a business type code."};
        }

        public override bool IsValid(object value)
        {
            return VaidateCode(value as string).IsValid;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var a = VaidateCode(value as string);

            if (a.IsValid)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(a.Message);
        }
    }
}
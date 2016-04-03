using System;
using System.ComponentModel.DataAnnotations;
using MarkEmbling.PostcodesIO;

namespace BookingBlock.WebApplication.Models.ValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property |
                    AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class PostcodeValidatorAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PostcodesIOClient client = new PostcodesIOClient();

            var result = client.Validate(value as string);

            if (result)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("The postcode given is not valid.");
        }
    }
}
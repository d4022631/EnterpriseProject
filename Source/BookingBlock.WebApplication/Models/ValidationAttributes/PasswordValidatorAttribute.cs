using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;

namespace BookingBlock.WebApplication.Models.ValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property |
                    AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class PasswordValidatorAttribute : ValidationAttribute
    {
        private readonly Type _passwordValidatorType;

        private Lazy<PasswordValidator> _passwordValidator;
        private IdentityResult passwordValidationResult;

        private PasswordValidator ValueFactory()
        {
            return Activator.CreateInstance(_passwordValidatorType) as PasswordValidator;
        }

        public PasswordValidatorAttribute(Type passwordValidatorType)
        {
            _passwordValidator = new Lazy<PasswordValidator>(ValueFactory);

            _passwordValidatorType = passwordValidatorType;
        }

        public override bool IsValid(object value)
        {
            string password = value as string;

            passwordValidationResult = _passwordValidator.Value.ValidateAsync(password).ConfigureAwait(false).GetAwaiter().GetResult();

            return passwordValidationResult.Succeeded;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string password = value as string;

            passwordValidationResult = _passwordValidator.Value.ValidateAsync(password).ConfigureAwait(false).GetAwaiter().GetResult();

            if (passwordValidationResult.Succeeded)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(string.Join(", ", passwordValidationResult.Errors));
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override string FormatErrorMessage(string name)
        {
            return base.FormatErrorMessage(name);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool IsDefaultAttribute()
        {
            return base.IsDefaultAttribute();
        }

        public override bool Match(object obj)
        {
            return base.Match(obj);
        }

        
    }
}
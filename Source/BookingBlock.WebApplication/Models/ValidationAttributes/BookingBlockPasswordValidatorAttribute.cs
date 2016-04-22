using BookingBlock.Identity;

namespace BookingBlock.WebApplication.Models.ValidationAttributes
{
    public class BookingBlockPasswordValidatorAttribute : PasswordValidatorAttribute
    {
        public BookingBlockPasswordValidatorAttribute() : base(typeof(BookingBlockPasswordValidator))
        {
        }
    }
}
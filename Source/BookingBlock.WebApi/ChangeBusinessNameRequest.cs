using Microsoft.Build.Framework;

namespace BookingBlock.WebApi
{
    public class ChangeBusinessNameRequest : ChangeBusinessRequest
    {

        /// <summary>
        /// The new name for the business.
        /// </summary>
        [Required]
        public string Name { get; set; }
    }
}
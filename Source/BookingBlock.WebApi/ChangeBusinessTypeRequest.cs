using Microsoft.Build.Framework;

namespace BookingBlock.WebApi
{
    public class ChangeBusinessTypeRequest : ChangeBusinessRequest
    {
        [Required]
        public string Type { get; set; }
    }
}
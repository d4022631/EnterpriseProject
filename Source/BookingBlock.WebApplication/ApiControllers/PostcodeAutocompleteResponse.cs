using System.Collections.Generic;

namespace BookingBlock.WebApplication.ApiControllers
{
    public class PostcodeAutocompleteResponse : PostcodeResponse
    {
        public List<string> Suggestions { get; set; } 
    }
}
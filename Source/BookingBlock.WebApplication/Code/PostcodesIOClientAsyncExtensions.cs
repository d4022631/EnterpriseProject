using System.Collections.Generic;
using System.Threading.Tasks;
using MarkEmbling.PostcodesIO;

namespace BookingBlock.WebApplication
{
    public static class PostcodesIOClientAsyncExtensions
    {
        public static async Task<IEnumerable<string>> AutocompleteAsync(this PostcodesIOClient client, string postcode)
        {
            return await Task.Run(() => client.Autocomplete(postcode));
        }

        public static async Task<bool> ValidateAsync(this PostcodesIOClient client, string postcode)
        {
            return await Task.Run(() => client.Validate(postcode));
        } 
    }
}
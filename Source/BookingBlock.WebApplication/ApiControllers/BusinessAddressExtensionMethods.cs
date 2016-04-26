using System.Collections.Generic;
using System.Threading.Tasks;
using BookingBlock.WebApi;

namespace BookingBlock.WebApplication.ApiControllers
{
    public static class BusinessAddressExtensionMethods
    {
        public static string GetAddressString(this IBusinessAddress businessAddress)
        {
            List<string> addressParts = new List<string>();

            addressParts.Add(businessAddress.AddressLine1);
            addressParts.Add(businessAddress.AddressLine2);
            addressParts.Add(businessAddress.TownCity);
            addressParts.Add(businessAddress.Postcode);
            addressParts.Add(businessAddress.Country);

            return string.Join(",\r\n", addressParts);
        }
    }
}
using System;
using Microsoft.Build.Framework;

namespace BookingBlock.WebApi
{
    public abstract class ChangeBusinessRequest
    {

        /// <summary>
        /// The id of the business we wish to change.
        /// </summary>
        [Required]
        public Guid BusinessId { get; set; }
    }

    public class ChangeBusinessTypeRequest : ChangeBusinessRequest
    {
        [Required]
        public string Type { get; set; }
    }

    public class ChangeBusinessNameRequest : ChangeBusinessRequest
    {

        /// <summary>
        /// The new name for the business.
        /// </summary>
        [Required]
        public string Name { get; set; }
    }

    /// <summary>
    /// Used to request a change of address for a business.
    /// </summary>
    public class ChangeBusinessAddressRequest : ChangeBusinessRequest, IBusinessAddress
    {
        /// <summary>
        /// The first line of the business' registerd address.
        /// </summary>
        public string AddressLine1 { get; set; }

        /// <summary>
        /// The second line of the business' registerd address.
        /// </summary>
        public string AddressLine2 { get; set; }

        /// <summary>
        /// The town/city of the business' registed address.
        /// </summary>
        public string TownCity { get; set; }

        /// <summary>
        /// The postcode of the business' registed address.
        /// </summary>
        public string Postcode { get; set; }

        /// <summary>
        /// The country of the business' registed address
        /// </summary>
        public string Country { get; set; }
    }
}
namespace BookingBlock.WebApi
{
    public interface IBusinessAddress
    {
        /// <summary>
        /// The first line of the business' registerd address.
        /// </summary>
        string AddressLine1 { get; set; }

        /// <summary>
        /// The second line of the business' registerd address.
        /// </summary>
        string AddressLine2 { get; set; }

        /// <summary>
        /// The town/city of the business' registed address.
        /// </summary>
        string TownCity { get; set; }

        /// <summary>
        /// The postcode of the business' registed address.
        /// </summary>
        string Postcode { get; set; }

        /// <summary>
        /// The country of the business' registed address
        /// </summary>
        string Country { get; set; }
    }
}
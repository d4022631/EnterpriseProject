using System;
using System.ComponentModel.DataAnnotations;

namespace BookingBlock.WebApi
{
    public class BusinessRegistrationData
    {
        /// <summary>
        /// The name of the business.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// The type of business being registered. E.g. plumbers.
        /// </summary>
        [Required]
        public string Type { get; set; }

        /// <summary>
        /// The name of the person to ask for when contacting the business, this could be the name of a person (e.g. Alice Smith) or more generic (e.g. Sales Department).
        /// </summary>
        [Required]
        public string ContactName { get; set; }

        /// <summary>
        /// An email address to contact the business via.
        /// </summary>
        [Required]
        public string ContactEmail { get; set; }

        /// <summary>
        /// A contact number for the business, this coud be a landline or mobile number.
        /// </summary>
        [Required]
        public string ContactNumber { get; set; }

        /// <summary>
        /// A Fax number for the business.
        /// </summary>
        public string ContactFax { get; set; }

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

        /// <summary>
        /// The website for the business.
        /// </summary>
        public string Website { get; set; }

        /// <summary>
        /// The email address of the account that should be set as the owner of the business.
        /// </summary>
        public string OwnerEmailAddress { get; set; }

        public TimeSpan? OpeningTimeMonday { get; set; }
        public TimeSpan? ClosingTimeMonday { get; set; }

        public TimeSpan? OpeningTimeTuesday { get; set; }
        public TimeSpan? ClosingTimeTuesday { get; set; }

        public TimeSpan? OpeningTimeWednesday { get; set; }
        public TimeSpan? ClosingTimeWednesday { get; set; }

        public TimeSpan? OpeningTimeThursday { get; set; }
        public TimeSpan? ClosingTimeThursday { get; set; }

        public TimeSpan? OpeningTimeFriday { get; set; }
        public TimeSpan? ClosingTimeFriday { get; set; }

        public TimeSpan? OpeningTimeSaturday { get; set; }
        public TimeSpan? ClosingTimeSaturday { get; set; }

        public TimeSpan? OpeningTimeSunday { get; set; }
        public TimeSpan? ClosingTimeSunday { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;

namespace BookingBlock.WebApi
{
    public class BusinessRegistrationData : IBusinessAddress
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

        private readonly OpeningTimes _openingTimes = new OpeningTimes();

        public OpeningTimes GetOpeningTimes()
        {
            return _openingTimes;
        }

        public TimeSpan? OpeningTimeMonday
        {
            get { return _openingTimes.GetOpeningTime(DayOfWeek.Monday); }
            set { _openingTimes.SetOpeningTime(DayOfWeek.Monday, value); }
        }

        public TimeSpan? ClosingTimeMonday
        {
            get { return _openingTimes.GetClosingTime(DayOfWeek.Monday); }
            set { _openingTimes.SetClosingTime(DayOfWeek.Monday, value); }
        }
        public TimeSpan? OpeningTimeTuesday
        {
            get { return _openingTimes.GetOpeningTime(DayOfWeek.Tuesday); }
            set { _openingTimes.SetOpeningTime(DayOfWeek.Tuesday, value); }
        }

        public TimeSpan? ClosingTimeTuesday
        {
            get { return _openingTimes.GetClosingTime(DayOfWeek.Tuesday); }
            set { _openingTimes.SetClosingTime(DayOfWeek.Tuesday, value); }
        }

        public TimeSpan? OpeningTimeWednesday
        {
            get { return _openingTimes.GetOpeningTime(DayOfWeek.Wednesday); }
            set { _openingTimes.SetOpeningTime(DayOfWeek.Wednesday, value); }
        }

        public TimeSpan? ClosingTimeWednesday
        {
            get { return _openingTimes.GetClosingTime(DayOfWeek.Wednesday); }
            set { _openingTimes.SetClosingTime(DayOfWeek.Wednesday, value); }
        }


        public TimeSpan? OpeningTimeThursday
        {
            get { return _openingTimes.GetOpeningTime(DayOfWeek.Thursday); }
            set { _openingTimes.SetOpeningTime(DayOfWeek.Thursday, value); }
        }

        public TimeSpan? ClosingTimeThursday
        {
            get { return _openingTimes.GetClosingTime(DayOfWeek.Thursday); }
            set { _openingTimes.SetClosingTime(DayOfWeek.Thursday, value); }
        }

        public TimeSpan? OpeningTimeFriday
        {
            get { return _openingTimes.GetOpeningTime(DayOfWeek.Friday); }
            set { _openingTimes.SetOpeningTime(DayOfWeek.Friday, value); }
        }

        public TimeSpan? ClosingTimeFriday
        {
            get { return _openingTimes.GetClosingTime(DayOfWeek.Friday); }
            set { _openingTimes.SetClosingTime(DayOfWeek.Friday, value); }
        }


        public TimeSpan? OpeningTimeSaturday
        {
            get { return _openingTimes.GetOpeningTime(DayOfWeek.Saturday); }
            set { _openingTimes.SetOpeningTime(DayOfWeek.Saturday, value); }
        }

        public TimeSpan? ClosingTimeSaturday
        {
            get { return _openingTimes.GetClosingTime(DayOfWeek.Saturday); }
            set { _openingTimes.SetClosingTime(DayOfWeek.Saturday, value); }
        }


        public TimeSpan? OpeningTimeSunday
        {
            get { return _openingTimes.GetOpeningTime(DayOfWeek.Sunday); }
            set { _openingTimes.SetOpeningTime(DayOfWeek.Sunday, value); }
        }

        public TimeSpan? ClosingTimeSunday
        {
            get { return _openingTimes.GetClosingTime(DayOfWeek.Sunday); }
            set { _openingTimes.SetClosingTime(DayOfWeek.Sunday, value); }
        }
    }
}
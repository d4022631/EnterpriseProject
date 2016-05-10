using System;

namespace BookingBlock.WebApi
{
    public class ChangeBusinessOpeningTimesRequest : ChangeBusinessRequest, IBusinessOpeningTimes
    {
        private readonly OpeningTimes _openingTimes = new OpeningTimes();

        public OpeningTimes GetOpeningTimes()
        {
            return _openingTimes;
        }

        public DateTime? OpeningTimeMonday
        {
            get { return _openingTimes.GetOpeningTime(DayOfWeek.Monday); }
            set { _openingTimes.SetOpeningTime(DayOfWeek.Monday, value); }
        }

        public DateTime? ClosingTimeMonday
        {
            get { return _openingTimes.GetClosingTime(DayOfWeek.Monday); }
            set { _openingTimes.SetClosingTime(DayOfWeek.Monday, value); }
        }
        public DateTime? OpeningTimeTuesday
        {
            get { return _openingTimes.GetOpeningTime(DayOfWeek.Tuesday); }
            set { _openingTimes.SetOpeningTime(DayOfWeek.Tuesday, value); }
        }

        public DateTime? ClosingTimeTuesday
        {
            get { return _openingTimes.GetClosingTime(DayOfWeek.Tuesday); }
            set { _openingTimes.SetClosingTime(DayOfWeek.Tuesday, value); }
        }

        public DateTime? OpeningTimeWednesday
        {
            get { return _openingTimes.GetOpeningTime(DayOfWeek.Wednesday); }
            set { _openingTimes.SetOpeningTime(DayOfWeek.Wednesday, value); }
        }

        public DateTime? ClosingTimeWednesday
        {
            get { return _openingTimes.GetClosingTime(DayOfWeek.Wednesday); }
            set { _openingTimes.SetClosingTime(DayOfWeek.Wednesday, value); }
        }


        public DateTime? OpeningTimeThursday
        {
            get { return _openingTimes.GetOpeningTime(DayOfWeek.Thursday); }
            set { _openingTimes.SetOpeningTime(DayOfWeek.Thursday, value); }
        }

        public DateTime? ClosingTimeThursday
        {
            get { return _openingTimes.GetClosingTime(DayOfWeek.Thursday); }
            set { _openingTimes.SetClosingTime(DayOfWeek.Thursday, value); }
        }

        public DateTime? OpeningTimeFriday
        {
            get { return _openingTimes.GetOpeningTime(DayOfWeek.Friday); }
            set { _openingTimes.SetOpeningTime(DayOfWeek.Friday, value); }
        }

        public DateTime? ClosingTimeFriday
        {
            get { return _openingTimes.GetClosingTime(DayOfWeek.Friday); }
            set { _openingTimes.SetClosingTime(DayOfWeek.Friday, value); }
        }


        public DateTime? OpeningTimeSaturday
        {
            get { return _openingTimes.GetOpeningTime(DayOfWeek.Saturday); }
            set { _openingTimes.SetOpeningTime(DayOfWeek.Saturday, value); }
        }

        public DateTime? ClosingTimeSaturday
        {
            get { return _openingTimes.GetClosingTime(DayOfWeek.Saturday); }
            set { _openingTimes.SetClosingTime(DayOfWeek.Saturday, value); }
        }


        public DateTime? OpeningTimeSunday
        {
            get { return _openingTimes.GetOpeningTime(DayOfWeek.Sunday); }
            set { _openingTimes.SetOpeningTime(DayOfWeek.Sunday, value); }
        }

        public DateTime? ClosingTimeSunday
        {
            get { return _openingTimes.GetClosingTime(DayOfWeek.Sunday); }
            set { _openingTimes.SetClosingTime(DayOfWeek.Sunday, value); }
        }
    }
}
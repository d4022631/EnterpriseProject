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
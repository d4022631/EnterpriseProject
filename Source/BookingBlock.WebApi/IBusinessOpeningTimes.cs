using System;

namespace BookingBlock.WebApi
{
    public interface IBusinessOpeningTimes
    {
        TimeSpan? OpeningTimeMonday { get; set; }
        TimeSpan? ClosingTimeMonday { get; set; }
        TimeSpan? OpeningTimeTuesday { get; set; }
        TimeSpan? ClosingTimeTuesday { get; set; }
        TimeSpan? OpeningTimeWednesday { get; set; }
        TimeSpan? ClosingTimeWednesday { get; set; }
        TimeSpan? OpeningTimeThursday { get; set; }
        TimeSpan? ClosingTimeThursday { get; set; }
        TimeSpan? OpeningTimeFriday { get; set; }
        TimeSpan? ClosingTimeFriday { get; set; }
        TimeSpan? OpeningTimeSaturday { get; set; }
        TimeSpan? ClosingTimeSaturday { get; set; }
        TimeSpan? OpeningTimeSunday { get; set; }
        TimeSpan? ClosingTimeSunday { get; set; }
    }
}
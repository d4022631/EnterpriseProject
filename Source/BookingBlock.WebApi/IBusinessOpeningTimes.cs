using System;

namespace BookingBlock.WebApi
{
    public interface IBusinessOpeningTimes
    {
        DateTime? OpeningTimeMonday { get; set; }
        DateTime? ClosingTimeMonday { get; set; }
        DateTime? OpeningTimeTuesday { get; set; }
        DateTime? ClosingTimeTuesday { get; set; }
        DateTime? OpeningTimeWednesday { get; set; }
        DateTime? ClosingTimeWednesday { get; set; }
        DateTime? OpeningTimeThursday { get; set; }
        DateTime? ClosingTimeThursday { get; set; }
        DateTime? OpeningTimeFriday { get; set; }
        DateTime? ClosingTimeFriday { get; set; }
        DateTime? OpeningTimeSaturday { get; set; }
        DateTime? ClosingTimeSaturday { get; set; }
        DateTime? OpeningTimeSunday { get; set; }
        DateTime? ClosingTimeSunday { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookingBlock.WebApplication.ApiControllers
{
    public class UserBusiness
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsOwner { get; set; }
    }

    public class UserBusinessList : List<UserBusiness>
    {
        
    }

    public class Business2
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Type { get; set; }

        public TimeSpan? OpeningTimeMonday { get; set; }
        public TimeSpan? OpeningTimeTuesday { get; set; }
        public TimeSpan? OpeningTimeWednesday { get; set; }
        public TimeSpan? OpeningTimeThursday { get; set; }
        public TimeSpan? OpeningTimeFriday { get; set; }
        public TimeSpan? OpeningTimeSaturday { get; set; }
        public TimeSpan? OpeningTimeSunday { get; set; }

        public TimeSpan? ClosingTimeMonday { get; set; }
        public TimeSpan? ClosingTimeTuesday { get; set; }
        public TimeSpan? ClosingTimeWednesday { get; set; }
        public TimeSpan? ClosingTimeThursday { get; set; }
        public TimeSpan? ClosingTimeFriday { get; set; }
        public TimeSpan? ClosingTimeSaturday { get; set; }
        public TimeSpan? ClosingTimeSunday { get; set; }
    }
}
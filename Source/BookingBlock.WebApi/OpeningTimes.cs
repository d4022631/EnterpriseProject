using System;
using System.Collections.Generic;

namespace BookingBlock.WebApi
{
    public sealed class OpeningTimes
    {
        private readonly Dictionary<DayOfWeek, Tuple<TimeSpan?, TimeSpan?>> _openingTimes = new Dictionary<DayOfWeek, Tuple<TimeSpan?, TimeSpan?>>(); 
        private readonly object _syncLock = new object();

        public TimeSpan? GetOpeningTime(DayOfWeek dayOfWeek)
        {
            lock (_syncLock)
            {
                return this[dayOfWeek].Item1;
            }
        }

        public void SetOpeningTime(DayOfWeek dayOfWeek, TimeSpan? openingTime)
        {
            lock (_syncLock)
            {
                var closingTime = GetClosingTime(dayOfWeek);

                this[dayOfWeek] = new Tuple<TimeSpan?, TimeSpan?>(openingTime, closingTime);
            }
        }

        public void SetClosingTime(DayOfWeek dayOfWeek, TimeSpan? closingTime)
        {
            lock (_syncLock)
            {
                var openingTime = GetOpeningTime(dayOfWeek);

                this[dayOfWeek] = new Tuple<TimeSpan?, TimeSpan?>(openingTime,closingTime);
            }
        }

        public TimeSpan? GetClosingTime(DayOfWeek dayOfWeek)
        {
            lock (_syncLock)
            {
                return this[dayOfWeek].Item2;
            }
        }

        public Tuple<TimeSpan?, TimeSpan?> this[DayOfWeek dayOfWeek]
        {
            get
            {
                lock (_syncLock)
                {
                    if (_openingTimes.ContainsKey(dayOfWeek))
                    {
                        return _openingTimes[dayOfWeek];
                    }
                }

                return new Tuple<TimeSpan?, TimeSpan?>(null, null);
            }

            set
            {
                lock (_syncLock)
                {
                    if (_openingTimes.ContainsKey(dayOfWeek))
                    {
                        _openingTimes[dayOfWeek] = value;
                    }
                    else
                    {
                        _openingTimes.Add(dayOfWeek, value);
                    }
                }
            }
        }
    }
}
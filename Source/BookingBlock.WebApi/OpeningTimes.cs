using System;
using System.Collections.Generic;

namespace BookingBlock.WebApi
{
    public sealed class OpeningTimes
    {
        private readonly Dictionary<DayOfWeek, Tuple<DateTime?, DateTime?>> _openingTimes = new Dictionary<DayOfWeek, Tuple<DateTime?, DateTime?>>(); 
        private readonly object _syncLock = new object();

        public DateTime? GetOpeningTime(DayOfWeek dayOfWeek)
        {
            lock (_syncLock)
            {
                return this[dayOfWeek].Item1;
            }
        }

        public void SetOpeningTime(DayOfWeek dayOfWeek, DateTime? openingTime)
        {
            lock (_syncLock)
            {
                var closingTime = GetClosingTime(dayOfWeek);

                this[dayOfWeek] = new Tuple<DateTime?, DateTime?>(openingTime, closingTime);
            }
        }

        public void SetClosingTime(DayOfWeek dayOfWeek, DateTime? closingTime)
        {
            lock (_syncLock)
            {
                var openingTime = GetOpeningTime(dayOfWeek);

                this[dayOfWeek] = new Tuple<DateTime?, DateTime?>(openingTime,closingTime);
            }
        }

        public DateTime? GetClosingTime(DayOfWeek dayOfWeek)
        {
            lock (_syncLock)
            {
                return this[dayOfWeek].Item2;
            }
        }

        public Tuple<DateTime?, DateTime?> this[DayOfWeek dayOfWeek]
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

                return new Tuple<DateTime?, DateTime?>(null, null);
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
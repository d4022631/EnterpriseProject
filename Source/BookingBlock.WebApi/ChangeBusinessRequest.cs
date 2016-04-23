using System;
using Microsoft.Build.Framework;

namespace BookingBlock.WebApi
{
    public abstract class ChangeBusinessRequest
    {

        /// <summary>
        /// The id of the business we wish to change.
        /// </summary>
        [Required]
        public Guid BusinessId { get; set; }
    }
}
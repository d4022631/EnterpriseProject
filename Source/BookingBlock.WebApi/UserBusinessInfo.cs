using System;

namespace BookingBlock.WebApi
{
    public class UserBusinessInfo
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsOwner { get; set; }
    }
}
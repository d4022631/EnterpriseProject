using System;
using System.Collections.Generic;

namespace BookingBlock.WebApplication.ApiControllers
{
    public class GenderDistribution
    {
        public int Males { get; set; }

        public int Females { get; set; }
    }

    public class BusinessTypesDistribution : List<BusinessTypeDistribution>
    {
        
    }

    public class BusinessTypeDistribution
    {
        public int Count { get; set; }

        public Guid BusinessTypeId { get; set; }

        public string BusinessTypeName { get; set; }
    }
}
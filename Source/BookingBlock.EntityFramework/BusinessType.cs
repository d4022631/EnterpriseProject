using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingBlock.EntityFramework
{
    public class BusinessType : BookingBlockEntity
    {


        [Required]
        public string Name { get; set; }

        public virtual ICollection<Business> Businesses { get; set; } 
    }
}
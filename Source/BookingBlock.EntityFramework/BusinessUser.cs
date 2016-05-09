using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingBlock.EntityFramework
{
    public class BusinessUser
    {

        public DateTime Created { get; set; } = DateTime.Now;

        public DateTime Modified { get; set; } = DateTime.Now;

        public bool Deleted { get; set; } = false;


        public virtual Business Business { get; set; }

        
        public virtual ApplicationUser User { get; set; }

        [Required]
        [Key, Column(Order = 0), ForeignKey(nameof(Business))]
        public Guid BusinessId { get; set; }

        [Required]
        [Key, Column(Order = 1), ForeignKey(nameof(User))]
        public string UserId { get; set; }

        [Required]
        public BusinessUserLevel UserLevel { get; set; }
    }
}
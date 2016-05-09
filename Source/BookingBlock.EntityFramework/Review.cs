using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingBlock.EntityFramework
{
    public class Review : BookingBlockEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        public virtual Business Business { get; set; }

        [Required, ForeignKey(nameof(Business))]
        public Guid BusinessId { get; set; }

        [Required]
        public int Rating { get; set; }

        [Required]
        public string Comments { get; set; }

    }
}
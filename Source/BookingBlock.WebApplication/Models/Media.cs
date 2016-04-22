using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookingBlock.EntityFramework;

namespace BookingBlock.WebApplication.Models
{
    /// <summary>
    /// Represents media that can be added to a business page, videos, pdfs, images, etc...
    /// </summary>
    public class Media
    { 
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Business Business {get; set;}

        [Required, ForeignKey(nameof(Business))]
        public Guid BusinessId {get; set;}

        public string Url {get; set;}
    
        public string Description {get; set;}

        public string Title {get; set;}
    }
}
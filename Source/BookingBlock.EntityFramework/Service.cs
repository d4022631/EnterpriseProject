using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace BookingBlock.EntityFramework
{
    public class ServiceStore
    {
        ApplicationDbContext _context;

        public ServiceStore(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Service> CreateAsync(Guid businessId, string name, string description, decimal cost,
            TimeSpan duration)
        {
            Business b = await _context.Businesses.FindAsync(businessId);

        
            var s = new Service()
            {
                Cost = cost,
                Description = description,
                Duration  = duration,
                Name =name
            };

            
            b.Services.Add(s);
            _context.SaveChanges();

            return s;
        }

        public async Task<string> GetNameAsync(Guid serviceId)
        {
            var service = await _context.Services.FindAsync(serviceId);

            if (service != null)
            {
                return service.Name;
            }

            return String.Empty;
        } 
    }

    public class Service : BookingBlockEntity
    {
        public Service()
        {
            this.Bookings = new List<Booking>();
        }



        public virtual Business Business { get; set; }

        [Required, ForeignKey(nameof(Business))]
        public Guid BusinessId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal Cost { get; set; } = 0.0M;

        [Required]
        public TimeSpan Duration { get; set; }



        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
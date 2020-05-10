using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Final.Models
{
    public class Order
    {
        [Required]
        public int Id { get; set; }
        
        [Required]
        public string CustomerId { get; set; }
        
        [Required]
        public int CartId { get; set; }
        
        [Required]
        public IdentityUser Customer { get; set; }
        
        [Required]
        public Cart Cart { get; set; }
        
        [Required]
        public string Address { get; set; }
        
        [Required]
        public string Phone { get; set; }
        
        [Required]
        public bool IsAccepted { get; set; }

    }
}
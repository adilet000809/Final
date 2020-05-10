using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Final.Models
{
    public class Cart
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string CustomerId { get; set; }
        [Required]
        public IdentityUser Customer { get; set; }
        
        public bool IsOrdered { get; set; }
        
    }
}
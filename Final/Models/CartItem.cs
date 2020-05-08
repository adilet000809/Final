using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Final.Models
{
    public class CartItem
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public Product Product { get; set; }
        [Required]
        public int CartId { get; set; }
        [Required]
        public Cart Cart { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace Final.Models
{
    public class CreateOrderViewModel
    {
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Address { get; set; }
    }
}
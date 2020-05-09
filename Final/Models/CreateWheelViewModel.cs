#nullable enable
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Final.Models
{
    public class CreateWheelViewModel
    {

        [Required]
        public string Name { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public double Width { get; set; }
        [Required]
        public int Diameter { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Hole { get; set; }
        [Required]
        public double HoleDiameter { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public virtual IFormFile Image { get; set; }
    }
}
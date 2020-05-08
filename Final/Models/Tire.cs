using System.ComponentModel.DataAnnotations;

namespace Final.Models
{
    public class Tire: Product
    {
        [Required]
        public int Width { get; set; }
        [Required]
        public int Height { get; set; }
        [Required]
        public string Diameter { get; set; }
        [Required]
        public Seasons Season { get; set; }
    }
}
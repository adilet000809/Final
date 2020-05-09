#nullable enable
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Final.Models
{
    public class CreateTireViewModel
    {

        [Required]
        public string Name { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int Width { get; set; }
        [Required]
        public int Height { get; set; }
        [Required]
        public string Diameter { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public Seasons Season { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public virtual IFormFile Image { get; set; }
    }
}
#nullable enable
using System.ComponentModel.DataAnnotations;

namespace Final.Models
{
    public class Product
    {
        [Required]
        public int Id { get; set; }
        [Display(Name = "Product")]
        [Required]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public Category Category { get; set; }
    }
}
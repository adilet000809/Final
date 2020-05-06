#nullable enable
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

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
        public int Discount { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public virtual Category Category { get; set; }
    }
}
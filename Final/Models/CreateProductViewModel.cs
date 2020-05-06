#nullable enable
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Final.Models
{
    public class CreateProductViewModel
    {

        [Required]
        public string Name { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public IFormFile Image { get; set; }
        [Required]
        public int Discount { get; set; }
        [Required]
        public int Amount { get; set; }
    }
}
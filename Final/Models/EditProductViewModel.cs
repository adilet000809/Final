using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Final.Models
{
    public class EditProductViewModel
    {

        public EditProductViewModel()
        {
            
        }
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Choose a category")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Enter price of the product")]
        public double Price { get; set; }
        public IFormFile Image { get; set; }
        [Required(ErrorMessage = "At least zero")]
        public int Discount { get; set; }
        public string OldImage { get; set; }
        [Required(ErrorMessage = "Amount is required")]
        public int Amount { get; set; }
        
    }
}
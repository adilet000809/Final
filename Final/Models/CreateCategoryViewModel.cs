using System.ComponentModel.DataAnnotations;

namespace Final.Models
{
    public class CreateCategoryViewModel
    {
        [Required]
        public string CategoryName { get; set; } 
    }
}
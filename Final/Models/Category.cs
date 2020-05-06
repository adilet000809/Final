using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Final.Models
{
    public class Category
    {

        [Required]
        public int Id { get; set; }
        [Display(Name = "Category")]
        [Required]
        public string Name { get; set; }
    }
}
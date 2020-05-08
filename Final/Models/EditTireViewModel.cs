using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Final.Models
{
    public class EditTireViewModel: CreateTireViewModel
    {
        
        public EditTireViewModel()
        {
            
        }
        public int Id { get; set; }

        public IFormFile Image { get; set; }
        public string OldImage { get; set; }
        
    }
}
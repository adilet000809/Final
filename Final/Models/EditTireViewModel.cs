using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Final.Models
{
    public class EditTireViewModel: CreateTireViewModel
    {
        
        public EditTireViewModel()
        {
            
        }
        public int Id { get; set; }
        [ValidateNever]

        public override IFormFile Image { get; set; }
        public string OldImage { get; set; }
        
    }
}
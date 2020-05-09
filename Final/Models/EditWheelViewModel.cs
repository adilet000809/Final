using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Final.Models
{
    public class EditWheelViewModel: CreateWheelViewModel
    {
        
        public EditWheelViewModel()
        {
            
        }
        public int Id { get; set; }
        [ValidateNever]
        public override IFormFile Image { get; set; }
        public string OldImage { get; set; }
        
    }
}
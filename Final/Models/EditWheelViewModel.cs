using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace Final.Models
{
    public class EditWheelViewModel: CreateWheelViewModel
    {
        
        public EditWheelViewModel()
        {
            
        }
        public int Id { get; set; }
        
        public IFormFile Image { get; set; }
        public string OldImage { get; set; }
        
    }
}
using System.Collections.Generic;

namespace Final.Models
{
    public class WheelPaginationViewModel
    {
        public IEnumerable<Wheel> Products { get; set; }
        public PaginationModel PaginationModel { get; set; }
    }
}
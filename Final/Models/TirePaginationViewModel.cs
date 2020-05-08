using System.Collections.Generic;

namespace Final.Models
{
    public class TirePaginationViewModel
    {
        public IEnumerable<Tire> Products { get; set; }
        public PaginationModel PaginationModel { get; set; }
    }
}
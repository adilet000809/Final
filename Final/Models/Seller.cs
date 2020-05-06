using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Final.Models
{
    public class Seller: IdentityUser
    {
        public List<Product> Products;

    }
}
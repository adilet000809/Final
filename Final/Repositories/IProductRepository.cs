using System.Collections.Generic;
using Final.Models;

namespace Final.Repositories
{
    public interface IProductRepository
    {
        Product GetProduct(int id);
        IEnumerable<Product> GetAllProduct();
        Product Add(Product product);
        Product Delete(int id);
        Product Update(Product product);
    }
}
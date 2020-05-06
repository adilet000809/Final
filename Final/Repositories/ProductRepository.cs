using System.Collections.Generic;
using Final.Data;
using Final.Models;

namespace Final.Repositories
{
    public class ProductRepository: IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public Product GetProduct(int id)
        {
            return _context.Products.Find(id);
        }

        public IEnumerable<Product> GetAllProduct()
        {
            return _context.Products;
        }

        public Product Add(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return product;
        }

        public Product Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null) return null;
            _context.Products.Remove(product);
            _context.SaveChanges();
            return product;
        }

        public Product Update(Product product)
        {
            var p = _context.Products.Attach(product);
            p.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return product;
        }
    }
}
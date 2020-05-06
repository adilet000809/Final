using System.Collections.Generic;
using System.Linq;
using Final.Data;
using Final.Models;

namespace Final.Repositories
{

    public class CategoryRepository: ICategoryRepository
    {

        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public Category GetCategory(int id)
        {
            return _context.Categories.Find(id);
        }

        public IEnumerable<Category> GetAllCategory()
        {
            return _context.Categories;
        }

        public Category Add(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
            return category;
        }

        public Category Update(Category category)
        {
            var c = _context.Categories.Attach(category);
            c.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return category;
        }

        public Category Delete(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null) return null;
            _context.Categories.Remove(category);
            _context.SaveChanges();

            return category;
        }
    }
}
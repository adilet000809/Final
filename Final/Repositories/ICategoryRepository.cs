using System.Collections.Generic;
using Final.Models;

namespace Final.Repositories
{
    public interface ICategoryRepository
    {
        Category GetCategory(int id);
        IEnumerable<Category> GetAllCategory();
        Category Add(Category category);
        Category Update(Category category);
        Category Delete(int id);
    }
}
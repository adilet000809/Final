using System.Collections.Generic;
using Final.Models;

namespace Final.Repositories
{
    public interface ICartRepository
    {
        Cart GetCart(int id);
        IEnumerable<Cart> GetAllCarts();
        Cart Add(Cart cart);
        Cart Delete(int id);
        Cart Update(Cart cart);

        void AddOrUpdate(Cart cart);
    }
}
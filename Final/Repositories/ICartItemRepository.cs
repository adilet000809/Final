using System.Collections.Generic;
using Final.Models;

namespace Final.Repositories
{
    public interface ICartItemRepository
    {
        CartItem GetCartItem(int id);
        IEnumerable<CartItem> GetAllCartItems();
        CartItem Add(CartItem cartItem);
        CartItem Delete(int id);
        CartItem Update(CartItem cartItem);

        void AddOrUpdate(CartItem cartItem);
    }
}
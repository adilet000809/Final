using System;
using System.Collections.Generic;
using Final.Data;
using Final.Models;
using Microsoft.EntityFrameworkCore;

namespace Final.Repositories
{
    public class CartItemRepository: ICartItemRepository
    {
        private readonly ApplicationDbContext _context;

        public CartItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public CartItem GetCartItem(int id)
        {
            return _context.CartItems.Find(id);
        }

        public IEnumerable<CartItem> GetAllCartItems()
        {
            return _context.CartItems;
        }

        public CartItem Add(CartItem cartItem)
        {
            _context.CartItems.Add(cartItem);
            _context.SaveChanges();
            return cartItem;
        }

        public CartItem Delete(int id)
        {
            var cartItem = _context.CartItems.Find(id);
            if (cartItem == null) return null;
            _context.CartItems.Remove(cartItem);
            _context.SaveChanges();
            return cartItem;
        }

        public CartItem Update(CartItem cartItem)
        {
            var c = _context.CartItems.Attach(cartItem);
            c.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return cartItem;
        }

        public void AddOrUpdate(CartItem cartItem)
        {
            var entry = _context.Entry(cartItem);
            switch (entry.State)  
            {  
                case EntityState.Detached:  
                    Add(cartItem);  
                    break;  
                case EntityState.Modified:  
                    Update(cartItem);  
                    break;  
                case EntityState.Added:  
                    Add(cartItem);  
                    break;  
                case EntityState.Unchanged:  
                    break;
                default:  
                    throw new ArgumentOutOfRangeException();  
            }  
        }
    }
}
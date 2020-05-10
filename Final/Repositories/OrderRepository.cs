using System.Collections.Generic;
using System.Linq;
using Final.Data;
using Final.Models;

namespace Final.Repositories
{

    public class OrderRepository: IOrderRepository
    {

        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public Order GetOrder(int id)
        {
            return _context.Orders.Find(id);
        }

        public IEnumerable<Order> GetAllOrder()
        {
            return _context.Orders;
        }

        public Order Add(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
            return order;
        }

        public Order Update(Order order)
        {
            var o = _context.Orders.Attach(order);
            o.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return order;
        }

        public Order Delete(int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null) return null;
            _context.Orders.Remove(order);
            _context.SaveChanges();

            return order;
        }
    }
}
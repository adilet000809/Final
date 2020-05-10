using System.Collections.Generic;
using Final.Models;

namespace Final.Repositories
{
    public interface IOrderRepository
    {
        Order GetOrder(int id);
        IEnumerable<Order> GetAllOrder();
        Order Add(Order order);
        Order Update(Order order);
        Order Delete(int id);
    }
}
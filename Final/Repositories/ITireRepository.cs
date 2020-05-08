using System.Collections.Generic;
using System.Threading.Tasks;
using Final.Controllers;
using Final.Models;

namespace Final.Repositories
{
    public interface ITireRepository
    {
        Tire GetTire(int id);
        IEnumerable<Tire> GetAllTires();
        Tire Add(Tire tire);
        Tire Delete(int id);
        Tire Update(Tire tire);
    }
}
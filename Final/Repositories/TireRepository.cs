using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Final.Data;
using Final.Models;

namespace Final.Repositories
{
    public class TireRepository: ITireRepository
    {
        private readonly ApplicationDbContext _context;

        public TireRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public Tire GetTire(int id)
        {
            return _context.Tires.Find(id);
        }

        public IEnumerable<Tire> GetAllTires()
        {
            return _context.Tires;
        }

        public Tire Add(Tire tire)
        {
            _context.Tires.Add(tire);
            _context.SaveChanges();
            return tire;
        }

        public Tire Delete(int id)
        {
            var tire = _context.Tires.Find(id);
            if (tire == null) return null;
            _context.Tires.Remove(tire);
            _context.SaveChanges();
            return tire;
        }

        public Tire Update(Tire tire)
        {
            var t = _context.Tires.Attach(tire);
            t.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return tire;
        }
    }
}
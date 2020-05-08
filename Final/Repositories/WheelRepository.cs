using System.Collections.Generic;
using Final.Data;
using Final.Models;

namespace Final.Repositories
{
    public class WheelRepository: IWheelRepository
    {
        private readonly ApplicationDbContext _context;

        public WheelRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public Wheel GetWheel(int id)
        {
            return _context.Wheels.Find(id);
        }

        public IEnumerable<Wheel> GetAllWheels()
        {
            return _context.Wheels;
        }

        public Wheel Add(Wheel wheel)
        {
            _context.Wheels.Add(wheel);
            _context.SaveChanges();
            return wheel;
        }

        public Wheel Delete(int id)
        {
            var wheel = _context.Wheels.Find(id);
            if (wheel == null) return null;
            _context.Wheels.Remove(wheel);
            _context.SaveChanges();
            return wheel;
        }

        public Wheel Update(Wheel wheel)
        {
            var w = _context.Wheels.Attach(wheel);
            w.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return wheel;
        }
    }
}
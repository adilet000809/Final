using System.Collections.Generic;
using Final.Models;

namespace Final.Repositories
{
    public interface IWheelRepository
    {
        Wheel GetWheel(int id);
        IEnumerable<Wheel> GetAllWheels();
        Wheel Add(Wheel wheel);
        Wheel Delete(int id);
        Wheel Update(Wheel wheel);
    }
}
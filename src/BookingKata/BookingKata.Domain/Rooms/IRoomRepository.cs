using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookingKata.Domain.Rooms
{
    public interface IRoomRepository
    {
        Task<IEnumerable<Room>> GetAllFreeRoomsAsync(AvailableRoomsQuery query);
    }
}

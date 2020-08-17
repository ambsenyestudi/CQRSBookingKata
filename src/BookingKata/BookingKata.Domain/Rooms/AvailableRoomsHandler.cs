using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BookingKata.Domain.Rooms
{
    public class AvailableRoomsHandler : IRequestHandler<AvailableRoomsQuery, IEnumerable<Room>>
    {
        private readonly IRoomRepository roomRepository;

        public AvailableRoomsHandler(IRoomRepository roomRepository)
        {
            this.roomRepository = roomRepository;
        }
        public async Task<IEnumerable<Room>> Handle(AvailableRoomsQuery query, CancellationToken cancellationToken)
        {
            //todo parameter
            var availablesRooms = await roomRepository.GetAllFreeRoomsAsync(query);
            return availablesRooms == null ?
                 new List<Room>():
                 availablesRooms;
        }
    }
}

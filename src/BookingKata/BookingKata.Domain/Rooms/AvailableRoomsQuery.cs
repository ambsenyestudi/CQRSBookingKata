using MediatR;
using System;
using System.Collections.Generic;

namespace BookingKata.Domain.Rooms
{
    public class AvailableRoomsQuery : IRequest<IEnumerable<Room>>
    {
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string Location { get; set; }
        public int NumberOfAdults { get; set; }
        public int NumberOfRoomsNeeded { get; set; }
        public int ChildrenCount { get; set; }
    }
}

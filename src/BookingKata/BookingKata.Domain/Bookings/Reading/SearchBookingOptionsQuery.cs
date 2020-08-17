using MediatR;
using System;
using System.Collections.Generic;

namespace BookingKata.Domain.Bookings.Reading
{
    public class SearchBookingOptionsQuery : IRequest<IEnumerable<BookingOption>>
    {
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string Location { get; set; }
        public int NumberOfAdults { get; set; }
        public int NumberOfRoomsNeeded { get; set; }
        public int ChildrenCount { get; set; }
    }
}

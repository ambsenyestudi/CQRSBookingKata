using System;

namespace BookingKata.Domain.Bookings
{
    public class Hotel
    {
        public string Location { get; }
        public string Name { get; }
        public Guid Id { get; }
        public int NumberOfRooms { get; }
        public Hotel(string location, string name, Guid id, int numberOfRooms)
        {
            Location = location;
            Name = name;
            Id = id;
            NumberOfRooms = numberOfRooms;
        }
    }
}

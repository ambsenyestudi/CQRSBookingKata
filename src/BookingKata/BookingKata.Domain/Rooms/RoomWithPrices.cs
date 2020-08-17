using System;
using System.Collections.Generic;
using System.Text;

namespace BookingKata.Domain.Rooms
{
    public class RoomWithPrices
    {
        public string Identifier { get; }
        public IEnumerable<Price> PriceCollection { get; set; }
        public RoomWithPrices(string identifier, IEnumerable<Price> priceCollection)
        {
            Identifier = identifier;
            PriceCollection = priceCollection;
        }
    }
}

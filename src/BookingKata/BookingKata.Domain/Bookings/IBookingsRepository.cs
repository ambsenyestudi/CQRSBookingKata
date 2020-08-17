using BookingKata.Domain.Bookings.Reading;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookingKata.Domain.Bookings
{
    public interface IBookingsRepository
    {
        Task<IEnumerable<BookingOption>> GetBookingOptionsAsync(BookingOptionsQuery query);
    }
}

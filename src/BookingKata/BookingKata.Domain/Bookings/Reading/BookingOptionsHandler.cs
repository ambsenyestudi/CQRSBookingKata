using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BookingKata.Domain.Bookings.Reading
{
    public class BookingOptionsHandler : IRequestHandler<BookingOptionsQuery, IEnumerable<BookingOption>>
    {
        private readonly IBookingsRepository bookingsRepository;

        public BookingOptionsHandler(IBookingsRepository bookingsRepository)
        {
            this.bookingsRepository = bookingsRepository;
        }
        public async Task<IEnumerable<BookingOption>> Handle(BookingOptionsQuery query, CancellationToken cancellationToken)
        {
            var bookingOptionCollection = await bookingsRepository.GetBookingOptionsAsync(query);
            return bookingOptionCollection == null ?
                 new List<BookingOption>() :
                 bookingOptionCollection;
        }
    }
}

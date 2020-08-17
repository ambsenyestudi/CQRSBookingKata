using FluentValidation;
using System;

namespace BookingKata.Domain.Bookings.Reading
{
    public class BookingOptionsValidator : AbstractValidator<BookingOptionsQuery>
    {
        private const int MIN_LOCATION_LETTERS = 3;
        public BookingOptionsValidator()
        {
            RuleFor(arq => arq.CheckInDate).GreaterThanOrEqualTo(DateTime.Today);
            RuleFor(arq => arq.CheckOutDate).GreaterThanOrEqualTo(DateTime.Today.AddDays(1));
            RuleFor(arq => arq.NumberOfAdults).GreaterThanOrEqualTo(1);
            RuleFor(arq => arq.NumberOfRoomsNeeded).GreaterThanOrEqualTo(1);
            RuleFor(arq => arq.ChildrenCount).GreaterThanOrEqualTo(0);
            RuleFor(arq => arq.Location).Must(LocationLongEnough);
        }

        private bool LocationLongEnough(string location) =>
            !string.IsNullOrWhiteSpace(location) &&
            location.Length >= MIN_LOCATION_LETTERS;
    }
}

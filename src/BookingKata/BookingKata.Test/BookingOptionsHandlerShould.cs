using BookingKata.Domain.Bookings;
using BookingKata.Domain.Bookings.Reading;
using BookingKata.Domain.Rooms;
using FluentValidation.TestHelper;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace BookingKata.Test
{
    public class BookingOptionsHandlerShould
    {
        private readonly IBookingsRepository repoMock;
        private readonly SeachBookingOptionsValidator validator;
        private readonly SearchBookingOptionsHandler sut;

        public BookingOptionsHandlerShould()
        {
            repoMock = Substitute.For<IBookingsRepository>();
            validator = new SeachBookingOptionsValidator();
            sut = new SearchBookingOptionsHandler(repoMock);
        }
        [Fact]
        public async Task GetEmptyWhenNoRooms()
        {
            repoMock.GetBookingOptionsAsync(default).ReturnsForAnyArgs(Task.FromResult(default(IEnumerable<BookingOption>)));
            var query = new SearchBookingOptionsQuery
            {
                CheckInDate = DateTime.Today,
                CheckOutDate = DateTime.Today.AddDays(5),
                ChildrenCount = 0,
                Location = "Here",
                NumberOfAdults = 2,
                NumberOfRoomsNeeded = 1
            };
            var roomCollection = await sut.Handle(query, CancellationToken.None);
            Assert.Empty(roomCollection);
        }

        [Fact]
        public void HaveAtlistTodayCheckIn()
        {
            var query = new SearchBookingOptionsQuery
            {
                CheckInDate = DateTime.Today.AddDays(-1),
                CheckOutDate = DateTime.Today.AddDays(5),
                ChildrenCount = 0,
                Location = "Here",
                NumberOfAdults = 2,
                NumberOfRoomsNeeded = 1
            };
            var result = validator.TestValidate(query);
            var failures = result.ShouldHaveValidationErrorFor(c => c.CheckInDate);
            Assert.NotEmpty(failures);
        }

        [Fact]
        public void HaveAtListOneDayStay()
        {
            var query = new SearchBookingOptionsQuery
            {
                CheckInDate = DateTime.Today,
                CheckOutDate = DateTime.Today,
                ChildrenCount = 0,
                Location = "Here",
                NumberOfAdults = 2,
                NumberOfRoomsNeeded = 1
            };
            var result = validator.TestValidate(query);
            var failures = result.ShouldHaveValidationErrorFor(c => c.CheckOutDate);
            Assert.NotEmpty(failures);
        }

        [Fact]
        public void HaveAnAdults()
        {
            var query = new SearchBookingOptionsQuery
            {
                CheckInDate = DateTime.Today,
                CheckOutDate = DateTime.Today.AddDays(1),
                ChildrenCount = 0,
                Location = "Here",
                NumberOfAdults = 0,
                NumberOfRoomsNeeded = 1
            };
            var result = validator.TestValidate(query);
            var failures = result.ShouldHaveValidationErrorFor(c => c.NumberOfAdults);
            Assert.NotEmpty(failures);
        }

        [Fact]
        public void HaveARoom()
        {
            var query = new SearchBookingOptionsQuery
            {
                CheckInDate = DateTime.Today,
                CheckOutDate = DateTime.Today.AddDays(1),
                ChildrenCount = 0,
                Location = "Here",
                NumberOfAdults = 1,
                NumberOfRoomsNeeded = 0
            };
            var result = validator.TestValidate(query);
            var failures = result.ShouldHaveValidationErrorFor(c => c.NumberOfRoomsNeeded);
            Assert.NotEmpty(failures);
        }

        [Fact]
        public void HaveNonOrSomeChildren()
        {
            var query = new SearchBookingOptionsQuery
            {
                CheckInDate = DateTime.Today,
                CheckOutDate = DateTime.Today.AddDays(1),
                ChildrenCount = -1,
                Location = "Here",
                NumberOfAdults = 1,
                NumberOfRoomsNeeded = 1
            };
            var result = validator.TestValidate(query);
            var failures = result.ShouldHaveValidationErrorFor(c => c.ChildrenCount);
            Assert.NotEmpty(failures);
        }

        [Fact]
        public void HaveLocation()
        {
            var query = new SearchBookingOptionsQuery
            {
                CheckInDate = DateTime.Today,
                CheckOutDate = DateTime.Today.AddDays(1),
                ChildrenCount = 0,
                Location = "xx",
                NumberOfAdults = 1,
                NumberOfRoomsNeeded = 1
            };
            var result = validator.TestValidate(query);
            var failures = result.ShouldHaveValidationErrorFor(c => c.Location);
            Assert.NotEmpty(failures);
        }

        [Fact]
        public async Task GetBookingOptions()
        {
            repoMock.GetBookingOptionsAsync(default).ReturnsForAnyArgs(Task.FromResult(CreateOptions()));
            var query = new SearchBookingOptionsQuery
            {
                CheckInDate = DateTime.Today,
                CheckOutDate = DateTime.Today.AddDays(1),
                ChildrenCount = 0,
                Location = "Panamá",
                NumberOfAdults = 1,
                NumberOfRoomsNeeded = 1
            };
            var result = await sut.Handle(query, CancellationToken.None);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void HaveKnowLocation()
        {
            Assert.True(false);
        }

        private IEnumerable<BookingOption> CreateOptions()
        {
            return new List<BookingOption>
            {
                new BookingOption(
                    new Hotel("Panamá", "El grandioso hotel", Guid.NewGuid(), 25),
                    new List<RoomWithPrices>
                    {
                        new RoomWithPrices(
                            "101",
                            new List<Price>
                            {
                                CreatePrice("OnePerson", "$16.0"),
                                CreatePrice("TwoPepeople", "$18.0")
                            }
                        )
                    }
                ),
                new BookingOption(
                    new Hotel("Panamá", "Hotel Lujos", Guid.NewGuid(), 45),
                    new List<RoomWithPrices>
                    {
                        new RoomWithPrices(
                            "701",
                            new List<Price>
                            {
                                CreatePrice("OnePerson", "$26.0"),
                                CreatePrice("TwoPepeople", "$28.0")
                            }
                        )
                    }
                ),
            };
        }

        private Price CreatePrice(string description, string price)
        {
            var (value, currency) = ProcessPrice(price);
            return new Price(value, currency, description);
        }

        private (double, string) ProcessPrice(string price)
        {
            var currency = price[0] + "";
            var value = price.Substring(1);
            if(double.TryParse(value, out double outValue))
            {
                return (outValue, currency);
            }
            return (10.0d, currency);

        }
    }
}

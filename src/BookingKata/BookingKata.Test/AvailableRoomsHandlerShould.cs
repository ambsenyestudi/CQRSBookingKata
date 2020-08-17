using BookingKata.Domain.Rooms;
using NSubstitute;
using System;
using FluentValidation;
using FluentValidation.TestHelper;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace BookingKata.Test
{
    public class AvailableRoomsHandlerShould
    {
        private readonly IRoomRepository repoMock;
        private readonly AvailableRoomsValidator validator;
        private readonly AvailableRoomsHandler sut;

        public AvailableRoomsHandlerShould()
        {
            repoMock = Substitute.For<IRoomRepository>();
            validator = new AvailableRoomsValidator();
            sut = new AvailableRoomsHandler(repoMock);
        }
        [Fact]
        public async Task GetEmptyWhenNoRooms()
        {
            repoMock.GetAllFreeRoomsAsync(default).ReturnsForAnyArgs(Task.FromResult(default(IEnumerable<Room>)));
            var query = new AvailableRoomsQuery
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
            var query = new AvailableRoomsQuery
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
            var query = new AvailableRoomsQuery
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
            var query = new AvailableRoomsQuery
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
            var query = new AvailableRoomsQuery
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
            var query = new AvailableRoomsQuery
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
            var query = new AvailableRoomsQuery
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
        public void HaveKnowLocation()
        {
            Assert.True(false);
        }
    }
}

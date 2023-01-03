using Hotel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace HotelTests
{
    [TestClass]
    public class HotelFacadeTests
    {
        private HotelFacade _hotelFacade;
        private Guid _guestId;
        private DateTime _from;
        private DateTime _to;

        [TestInitialize]
        public void Initialize()
        {
            _hotelFacade = new HotelFacade(new ApartamentsInMemoryStorage(), new BookingInMemoryStorage(), new GuestInMemoryStorage(), new LuggageRoom (100));
            _guestId = _hotelFacade.AddGuest("Петька", "Пупкин",12);
            _from = new DateTime(2001, 01, 01);
            _to = new DateTime(2001, 01, 02);
        }

        [TestMethod]
        public void BookingApartament_AddBooking()
        {
            //prepare
            int roomsCount = 1;
            _hotelFacade.AddApartament(roomsCount);

            //act
            var booking = _hotelFacade.TryBookApartament(_guestId, _from, _to,roomsCount);

            //validation
            var expectedResult = true;
            var expectendCount = 1;

            Assert.AreEqual(expectedResult, booking);
            Assert.AreEqual(expectendCount, _hotelFacade.GetAllBookings().Count);
        }

        [TestMethod]
        public void BookingApartament_ReturnFalse()
        {
            //prepare
            int roomsCount = 1;

            _hotelFacade.AddApartament(roomsCount);
            _hotelFacade.TryBookApartament(_guestId, _from, _to, roomsCount);

            //act
            var booking = _hotelFacade.TryBookApartament(_guestId, _from, _to, roomsCount);

            //validation
            var expectedResult = false;
            var expectendCount = 1;

            Assert.AreEqual(expectedResult, booking);
            Assert.AreEqual(expectendCount, _hotelFacade.GetAllBookings().Count);
        }

        [TestMethod]
        public void BookingApartament_ReturnTrue()
        {
           //prepare
            var roomsCount = 1;
            var from = new DateTime(2001, 01, 02);
            var to = new DateTime(2001, 01, 03);

            _hotelFacade.AddApartament(roomsCount);
            _hotelFacade.TryBookApartament(_guestId, _from, _to, roomsCount);


            //act
            var booking = _hotelFacade.TryBookApartament(_guestId, from, to, roomsCount);

            //validation
            var expectedResult = true;
            var expectendCount = 2;

            Assert.AreEqual(expectedResult, booking);
            Assert.AreEqual(expectendCount, _hotelFacade.GetAllBookings().Count);
        }

        [TestMethod]
        public void BookingNotExistApartment_ReturnFalse()
        {
            //prepare
            var roomsCount = 2;
            var from = new DateTime(2001, 01, 02);
            var to = new DateTime(2001, 01, 03);

            _hotelFacade.AddApartament(roomsCount);

            //act
            var booking = _hotelFacade.TryBookApartament(_guestId, from, to, 1);

            //validation
            var expectedResult = false;
            var expectendCount = 0;

            Assert.AreEqual(expectedResult, booking);
            Assert.AreEqual(expectendCount, _hotelFacade.GetAllBookings().Count);
        }

        [TestMethod]
        public void TryPopulateGuest_ReturnTrue()
        {
            //prepare
            var roomsCount = 1;

            _hotelFacade.AddApartament(roomsCount);
            _hotelFacade.TryBookApartament(_guestId, _from, _to, roomsCount);

            //act
            var result = _hotelFacade.TryPopulateGuest(_guestId, _from, _to);

            //validation
            var expectedResult = true;
            var expectendCount = 1;

            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(expectendCount, _hotelFacade.GetAllBookings().Count);
        }

        [TestMethod]
        public void TryInstantBookApartament_ReturnTrue()
        {
            //prepare
            var roomsCount = 1;

            _hotelFacade.AddApartament(roomsCount);

            //act
            var result = _hotelFacade.TryPopulateApartament(_guestId, _from, _to, roomsCount);

            //validation
            var expectedResult = true;
            var expectendCount = 1;

            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(expectendCount, _hotelFacade.GetAllBookings().Count);
        }
    }
}

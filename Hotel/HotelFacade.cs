using System;
using System.Collections.Generic;

namespace Hotel
{
    public class HotelFacade
    {
        public HotelFacade(ApartamentsInMemoryStorage apartamentsStorage, BookingInMemoryStorage bookingStorage, GuestInMemoryStorage guestStorage, LuggageRoom lugageRoom)
        {
            _apartamentsStorage = apartamentsStorage;
            _bookingStorage = bookingStorage;
            _guestStorage = guestStorage;
            _luggageRoom = lugageRoom;
        }

        private ApartamentsInMemoryStorage _apartamentsStorage;
        private BookingInMemoryStorage _bookingStorage;
        private GuestInMemoryStorage _guestStorage;
        private LuggageRoom _luggageRoom;

        public Guid AddGuest(string name, string surname, int luggageCount)
        {
           return _guestStorage.Add(name, surname, luggageCount);
        }

        public void AddApartament(int roomCount)
        {
            _apartamentsStorage.Add(roomCount);    
        }

        public bool TryBookApartament(Guid guestId, DateTime from, DateTime to, int roomsCount)
        {
            if (_guestStorage.CheckInBlacklist(guestId) == false)
            {
                foreach (var apartament in _apartamentsStorage.GetAll(roomsCount))
                {
                    if (_bookingStorage.TryBookingApartament(guestId, from, to, apartament.Id))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool TryPopulateApartament(Guid guestId, DateTime from, DateTime to, int roomsCount)
        {
            if (_guestStorage.CheckInBlacklist(guestId) == false)
            {
                foreach (var apartament in _apartamentsStorage.GetAll(roomsCount))
                {
                    if (_bookingStorage.TryPopulateApartament(guestId, from, to, apartament.Id))
                    {
                        return true;
                    }
                }
            }
            return false;

        }

        public bool TryPopulateGuest(Guid guestId, DateTime from, DateTime to)
        {
            return _bookingStorage.TryPopulateGuest(guestId, from, to);
        }

        public List<Guid> GetAllBlokedGuests() 
        {
            return _guestStorage.GetAllBlockedGuests();
        }

        public List<Booking> GetAllBookings()
        {
            return _bookingStorage.GetAll();
        }

        public bool DeleteBooking(Guid guestId)
        {
            var result = _bookingStorage.Delete(guestId);
            return result;
        }

        public bool AddGuestInBlackList(Guid guestId)
        {
            return _guestStorage.AddInBlacklist(guestId);
        }

        public Dictionary<Guid, int> GetAllLuggages()
        {
            return _luggageRoom.GetAllLuggages();
        }
    }
}

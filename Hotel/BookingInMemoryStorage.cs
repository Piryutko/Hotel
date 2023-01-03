using System;
using System.Collections.Generic;
using System.Linq;

namespace Hotel
{
    public class BookingInMemoryStorage : IBookingStorage
    {
        public BookingInMemoryStorage()
        {
            _booked = new List<Booking>();   
        }

        private readonly List<Booking> _booked;

        public bool TryBookingApartament(Guid guestId, DateTime from, DateTime to, Guid apartamentId)
        {
            var succeeded = false;

            if (ExistsApartament(apartamentId) == false)
            {
                Add(guestId, apartamentId, from, to);
                return true;
            }

            foreach (var booking in _booked)
            {
                if (from < booking.From && to <= booking.To || from >= booking.To && to > booking.To)
                {
                    succeeded = true;
                }
                else
                {
                    succeeded = false;
                    break;
                }
            }

            if (succeeded)
            {
                Add(guestId, apartamentId, from, to);
            }

            return succeeded;
        }

        public bool TryPopulateApartament(Guid guestId, DateTime from, DateTime to, Guid apartamentId)
        {
            var succeeded = false;

            if (ExistsApartament(apartamentId) == false)
            {
                InstantAddOccupied(guestId, apartamentId, from, to);
                return true;
            }

            foreach (var apartament in _booked)
            {
                if (from < apartament.From && to <= apartament.To || from >= apartament.To && to > apartament.To)
                {
                    succeeded = true;
                }
                else
                {
                    succeeded = false;
                    break;
                }
            }

            if (succeeded)
            {
                InstantAddOccupied(guestId, apartamentId, from, to);
            }

            return succeeded;
        }

        private void Add(Guid guestId, Guid apartamentId, DateTime from, DateTime to)
        {
            var booking = new Booking(guestId,apartamentId, from, to);
            _booked.Add(booking);
        }

        private void InstantAddOccupied(Guid guestId, Guid apartamentId, DateTime from, DateTime to)
        {
            var booking = new Booking(guestId, apartamentId, from, to);
            booking.ChangeStatusOccupied(1);
            _booked.Add(booking);
        }

        public bool Delete(Guid userId)
        {
            var result = _booked.Any(b => b.GuestId == userId);

            if (result)
            {
                var booking = _booked.Single(b => b.GuestId == userId);
                _booked.Remove(booking);
            }

            return result;
        }

        public List<Booking> GetAll()
        {
            return _booked;
        }

        public bool HasBooking(Guid guestId, DateTime from, DateTime to, int status, out Booking booking)
        {
            booking = null;

            if (_booked.Any(n => n.GuestId == guestId && n.From == from && n.To == to && n.IsRoomOccupied == status))
            {
                booking = GetAll().Single(n => n.GuestId == guestId && n.From == from && n.To == to && n.IsRoomOccupied == status);
                return true;
            }
            return false;
        }

        public bool ExistsApartament(Guid id)
        {
            return GetAll().Any(a => a.ApartamentId == id);
        }

        public bool TryPopulateGuest(Guid guestId, DateTime from, DateTime to)
        {
            if (HasBooking(guestId, from, to, 0, out Booking booking))
            {
                booking.ChangeStatusOccupied(1);
                return true;
            }
            return false;
        }
    }
}

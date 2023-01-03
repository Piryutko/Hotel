using System;
using System.Collections.Generic;

namespace Hotel
{
    interface IBookingStorage
    {
        public bool TryBookingApartament(Guid guestId, DateTime from, DateTime to, Guid apartamentId);

        public bool TryPopulateApartament(Guid guestId, DateTime from, DateTime to, Guid apartamentId);

        public bool Delete(Guid userId);

        public List<Booking> GetAll();

        public bool HasBooking(Guid guestId, DateTime from, DateTime to, int status, out Booking booking);

        public bool ExistsApartament(Guid id);
    }
}

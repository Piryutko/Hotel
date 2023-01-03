using System;

namespace Hotel
{
    public class Booking
    {
        public Booking(Guid guestId, Guid apartamentId, DateTime from, DateTime to)
        {
            GuestId = guestId;
            ApartamentId = apartamentId;
            From = from;
            To = to;
            Id = Guid.NewGuid();
            IsRoomOccupied = 0;
        }

        public Booking(Guid guestId, Guid apartamentId, DateTime from, DateTime to,Guid id,int isRoomOccupied)
        {
            GuestId = guestId;
            ApartamentId = apartamentId;
            From = from;
            To = to;
            Id = id;
            IsRoomOccupied = isRoomOccupied;
        }

        public Guid Id { get; }

        public Guid GuestId { get; }

        public Guid ApartamentId { get; }

        public DateTime From { get; }

        public DateTime To { get; }

        public int IsRoomOccupied { get; private set; }

        public int ChangeStatusOccupied(int newStatus)
        {
           return IsRoomOccupied = newStatus;
        }
    }
}

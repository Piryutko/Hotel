using System;

namespace Hotel
{
    public class Apartment
    {
        public Apartment(int rooms)
        {
            Id = Guid.NewGuid();
            RoomsCount = rooms;
        }

        public Apartment(Guid id,int roomsCount)
        {
            Id = id;
            RoomsCount = roomsCount;
        }

        public Guid Id { get; }
        public int RoomsCount { get; }
    }
}

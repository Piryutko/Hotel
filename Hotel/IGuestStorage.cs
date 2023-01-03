using System;
using System.Collections.Generic;

namespace Hotel
{
    interface IGuestStorage
    {
        public Guid Add(string name, string surname, int luggage);

        public bool AddInBlacklist(Guid guestId);

        public bool Delete(Guid guestId);

        public bool CheckInBlacklist(Guid guestId);

        public List<Guid> GetAllBlockedGuests();
    }
}

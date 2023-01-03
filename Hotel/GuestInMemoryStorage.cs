using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel
{
    public class GuestInMemoryStorage : IGuestStorage
    {
        public GuestInMemoryStorage()
        {
            _allGuests = new List<Guest>();
            _blacklistedGuests = new List<Guid>();
        }

        private List<Guest> _allGuests; 

        private List<Guid> _blacklistedGuests;


        public Guid Add(string name, string surname, int luggage)
        {
            var guest = new Guest(name, surname, luggage);
            _allGuests.Add(guest);
            return guest.Id;
        }

        public bool AddInBlacklist(Guid guestId) 
        {
            var result = _allGuests.Any(g => g.Id == guestId);
            if (result)
            {
                _blacklistedGuests.Add(guestId);
                return result;
            }
            return result;
        }

        public bool Delete(Guid guestId)
        {
            var isGuest = _allGuests.Any(g => g.Id == guestId);
            if (isGuest)
            {
                _allGuests.RemoveAll(g => g.Id == guestId);
            }
            return isGuest;
        }

        public bool CheckInBlacklist(Guid guestId)
        {
            var result = _blacklistedGuests.Any(g => g == guestId);

            return result;
        }

        public List<Guid> GetAllBlockedGuests()
        {
            return _blacklistedGuests;
        }
    }
}

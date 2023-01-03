using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel
{
    public class LuggageRoom
    {
        public LuggageRoom(int places)
        {
            Places = places;
            _luggagesCount = new Dictionary<Guid, int>();
        }

        private int Places { get; } //
        
        public int AllPlaces { get; private set; } //

        private Dictionary<Guid, int> _luggagesCount; //

        public void AddLuggage(int luggageCountToAdd, Guid guestId)
        {
            
        }
        
        private bool CheckLuggageRoom(int luggage)
        {
            return false;
        }

        public Dictionary<Guid, int> GetAllLuggages()
        {
            return _luggagesCount;
        }
    }
}

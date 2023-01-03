using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel
{
    interface IApartamentsStorage
    {
        public Guid Add(int roomCount);

        public bool Delete(Guid apartamentId);

        public bool CheckExistance(Guid apartId);

        public bool TryFind(int roomsCount, out Guid apartmentId);

        public List<Apartment> GetAll(int roomsCount);

    }
}

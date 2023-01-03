using System;
using System.Collections.Generic;
using System.Linq;

namespace Hotel
{
    public class ApartamentsInMemoryStorage : IApartamentsStorage
    {
        public ApartamentsInMemoryStorage()
        {
            _apartaments = new List<Apartment>();
        }

        private List<Apartment> _apartaments;

        public Guid Add(int roomCount)
        {
            var apartament = new Apartment(roomCount);
            _apartaments.Add(apartament);
            return apartament.Id;
        }

        public bool Delete(Guid apartamentId)
        {
            var isApartament = _apartaments.Any(a => a.Id == apartamentId) ? _apartaments.Remove(_apartaments.Single(a => a.Id == apartamentId)) : false;

            return isApartament;
        }

        public bool CheckExistance(Guid apartId)
        {
            return _apartaments.Any(a => a.Id == apartId);
        }

        public bool TryFind(int roomsCount, out Guid apartmentId)
        {
            apartmentId = Guid.Empty;
            var suitableApartments = _apartaments.FindAll(apartment => apartment.RoomsCount == roomsCount);

            if (suitableApartments.Count > 0)
            {
                apartmentId = suitableApartments.First().Id;
                return true;
            }

            return false;
        }

        public List<Apartment> GetAll(int roomsCount)
        {
            var apartaments = _apartaments.Where(a => a.RoomsCount == roomsCount).ToList();
            return apartaments;
        }

    }
}

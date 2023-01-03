using Hotel;
using System;
using LinqToDB;
using Microsoft.Data.SqlClient;

namespace HotelConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=DESKTOP-O6GTIKV;Database=HotelDb;Trusted_Connection=True;TrustServerCertificate=Yes;";

            var apartamentStorage = new ApartamentStorage(connectionString);

            var guestStorage = new GuestStorage(connectionString);

            var bookingStorage = new BookingStorage(connectionString);

            var guestId = Guid.Parse("021AE749-5741-4A70-A45E-BD31DC482140");
            var apartamentId = Guid.Parse("611A227F-9271-4438-9038-C8F0C7AF8BD0");
            var from = new DateTime(2001,01,03);
            var to = new DateTime(2001,01,05);


            


            


        }
    }
}

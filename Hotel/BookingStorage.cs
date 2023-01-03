using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;


namespace Hotel
{
    public class BookingStorage : IBookingStorage
    {
        public BookingStorage(string connectionString)
        {
            _connectionString = connectionString;
        }

        private readonly string _connectionString;

        public void Add(Guid guestId, Guid apartamentId, DateTime from, DateTime to)
        {
            try
            {
                var connection = new SqlConnection(_connectionString);
                connection.Open();

                var idBooking = Guid.NewGuid();
                var isRoomOccupied = 0;

                var insertCommand = $"INSERT Bookings(Guest_Id,Apartament_Id, [From], [To], Id, IsRoomOccupied)" +
                    $" VALUES({"'" + guestId + "'"}, {"'" + apartamentId + "'"}, {"'" + from + "'"}, {"'" + to + "'"}" +
                    $",{"'" + idBooking + "'"}, {isRoomOccupied})";

                var command = new SqlCommand(insertCommand, connection);
                command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                
            }

        }

        private void AddOccupied(Guid guestId, Guid apartamentId, DateTime from, DateTime to)
        {
            try
            {
                var connection = new SqlConnection(_connectionString);
                connection.Open();

                var idBooking = Guid.NewGuid();
                var isRoomOccupied = 1;

                var insertCommand = $"INSERT Bookings(Guest_Id,Apartament_Id, [From], [To], Id, IsRoomOccupied)" +
                    $" VALUES({"'" + guestId + "'"}, {"'" + apartamentId + "'"}, {"'" + from.Year + "'"}, {"'" + to.Year + "'"}" +
                    $",{"'" + idBooking + "'"}, {isRoomOccupied})";

                var command = new SqlCommand(insertCommand, connection);
                command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                
            }
        }

        public bool Delete(Guid userId)
        {
            try
            {
                var connection = new SqlConnection(_connectionString);
                connection.Open();

                var deleteCommand = $"DELETE Bookings WHERE Id = {"'" + userId + "'"}";
                var command = new SqlCommand(deleteCommand, connection);
                return command.ExecuteNonQuery() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<Booking> GetAll()
        {
            var bookings = new List<Booking>();
            try
            {
                var connection = new SqlConnection(_connectionString);
                connection.Open();

                var selectCommand = "SELECT * FROM Bookings";
                var command = new SqlCommand(selectCommand, connection);

                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var guestId = (Guid)reader["Guest_Id"];
                        var apartamentId = (Guid)reader["Apartament_Id"];
                        var from = (DateTime)reader["From"];
                        var to = (DateTime)reader["To"];
                        var isRoomOccupied = (int)reader["IsRoomOccupied"];
                        var id = (Guid)reader["Id"];

                        var booking = new Booking(guestId, apartamentId, from, to, id, isRoomOccupied);

                        bookings.Add(booking);
                    }
                    return bookings;
                }

                return bookings;
            }
            catch (Exception)
            {
                return bookings;
            }
        }

        public List<Booking> GetAll(Guid apartamentId)
        {
            var bookings = new List<Booking>();
            try
            {
                var connection = new SqlConnection(_connectionString);
                connection.Open();

                var selectCommand = $"SELECT * FROM Bookings WHERE Apartament_Id = '{apartamentId}'";
                var command = new SqlCommand(selectCommand, connection);

                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var guestId = (Guid)reader["Guest_Id"];
                        var apartament_Id = (Guid)reader["Apartament_Id"];
                        var from = (DateTime)reader["From"];
                        var to = (DateTime)reader["To"];
                        var isRoomOccupied = (int)reader["IsRoomOccupied"];
                        var id = (Guid)reader["Id"];

                        var booking = new Booking(guestId, apartament_Id, from, to, id, isRoomOccupied);

                        bookings.Add(booking);
                    }
                    return bookings;
                }

                return bookings;
            }
            catch (Exception)
            {
                return bookings;
            }
        }

        public bool HasBooking(Guid guestId, DateTime from, DateTime to, int status, out Booking booking)
        {
            booking = null;

            try
            {
                var connection = new SqlConnection(_connectionString);
                connection.Open();

                var filterCommand = $"SELECT * FROM Bookings " +
                    $"WHERE Guest_Id = '{guestId}' AND[From] = '{from}' AND[To] = '{to}' AND IsRoomOccupied = '{status}'";
                var command = new SqlCommand(filterCommand, connection);

                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var bookingId = (Guid)reader["Id"];
                        var apartamentId = (Guid)reader["Apartament_Id"];
                        booking = new Booking(guestId,apartamentId,from,to,bookingId,status);
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ExistsApartament(Guid id)
        {
            try
            {
                var connection = new SqlConnection(_connectionString);
                connection.Open();

                var filtrCommand = $"SELECT * FROM Bookings WHERE Apartament_Id = '{id}'";

                var command = new SqlCommand(filtrCommand, connection);

                var result = command.ExecuteReader();

                return result.HasRows;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool TryBookingApartament(Guid guestId, DateTime from, DateTime to, Guid apartamentId)
        {
            try
            {
                var succeeded = false;

                if (ExistsApartament(apartamentId) == false)
                {
                    Add(guestId, apartamentId, from, to);
                    return true;
                }

                foreach (var booking in GetAll(apartamentId))
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
            catch (Exception)
            {
                return false;
            }
            
        }

        public bool TryPopulateApartament(Guid guestId, DateTime from, DateTime to, Guid apartamentId)
        {
            try
            {
                var succeeded = false;

                if (TryPopulateGuest(guestId, from, to))
                {
                    return true;
                }

                if (ExistsApartament(apartamentId) == false)
                {
                    AddOccupied(guestId, apartamentId, from, to);
                    return true;
                }

                foreach (var booking in GetAll(apartamentId))
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
                    AddOccupied(guestId, apartamentId, from, to);
                }

                return succeeded;

            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool TryPopulateGuest(Guid guestId, DateTime from, DateTime to)
        {
            try
            {
                if (HasBooking(guestId, from, to, 0, out Booking booking))
                {
                    var connection = new SqlConnection(_connectionString);
                    connection.Open();

                    var updateCommand = $"UPDATE Bookings SET IsRoomOccupied = 1 WHERE Id = '{booking.Id}'";
                    var command = new SqlCommand(updateCommand, connection);
                    command.ExecuteNonQuery();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
            
        }
    }
}

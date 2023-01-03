using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;

namespace Hotel
{
   public class GuestStorage : IGuestStorage
    {
        public GuestStorage(string connectionString)
        {
            _connectionString = connectionString;
        }

        private readonly string _connectionString;

        public Guid Add(string name, string surname, int luggage)
        {
            try
            {
                var connection = new SqlConnection(_connectionString);
                connection.Open();

                var guest = new Guest(name, surname, luggage);

                var insertCommand = $"INSERT Guests(Name,Luggage_Count,Surname,Id) " +
                $"VALUES('{name}',{luggage},'{surname}','{guest.Id}')";

                var command = new SqlCommand(insertCommand, connection);
                command.ExecuteNonQuery();

                return guest.Id;
            }
            catch (Exception)
            {
                return Guid.Empty;
            }
        }

        public bool AddInBlacklist(Guid guestId)
        {
            try
            {
                var connection = new SqlConnection(_connectionString);
                connection.Open();

                var filterCommand = $"SELECT * FROM Guests WHERE Id = '{guestId}'";
                var command = new SqlCommand(filterCommand, connection);

                var result = command.ExecuteReader();

                if (result.HasRows)
                {
                    result.Close();

                    var insertCommand = $"INSERT Blocked_Guests(Id) VALUES ('{guestId}')";
                    var sqlCommand = new SqlCommand(insertCommand, connection);
                    sqlCommand.ExecuteNonQuery();

                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete(Guid guestId)
        {
            try
            {
                var connection = new SqlConnection(_connectionString);
                connection.Open();

                var commandString = $"DELETE FROM Guests WHERE Id = '{guestId}'";

                var command = new SqlCommand(commandString, connection);

                var result = command.ExecuteNonQuery();

                return result > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CheckInBlacklist(Guid guestId)
        {
            try
            {
                var connection = new SqlConnection(_connectionString);
                connection.Open();

                var filtrCommand = $"SELECT * FROM Blocked_Guests WHERE Id = '{guestId}'";

                var command = new SqlCommand(filtrCommand, connection);
                var result = command.ExecuteReader();

                return result.HasRows;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public List<Guid> GetAllBlockedGuests()
        {
            var blockedGuests = new List<Guid>();

            try
            {
                var connection = new SqlConnection(_connectionString);
                connection.Open();

                var filterCommand = $"SELECT Id FROM Blocked_Guests";
                var command = new SqlCommand(filterCommand, connection);

                var result = command.ExecuteReader();

                if (result.HasRows)
                {
                    while (result.Read())
                    {
                        var id = result["Id"];
                        blockedGuests.Add((Guid)id);
                    }
                }

                return blockedGuests;

            }
            catch (Exception)
            {
                return blockedGuests;
            }
        }
    }
}

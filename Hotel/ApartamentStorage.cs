using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace Hotel
{
    public class ApartamentStorage : IApartamentsStorage
    {
        public ApartamentStorage(string connectionString)
        {
            _connectionString = connectionString;
        }

        private readonly string _connectionString;

        public Guid Add(int roomCount)
        {
            var apartament = new Apartment(roomCount);

            try
            {
                var connection = new SqlConnection(_connectionString);
                connection.Open();

                string insertCommand = $"INSERT Apartaments(Id, rooms_count) VALUES ('{apartament.Id}',{apartament.RoomsCount})";
                var command = new SqlCommand(insertCommand, connection);
                command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                return Guid.Empty;
            }

            return apartament.Id;
        }

        public bool CheckExistance(Guid apartamentId)
        {
            try
            {
                var connection = new SqlConnection(_connectionString);
                connection.Open();

                var filterCommand = $"SELECT * FROM Apartaments WHERE Id = '{apartamentId}'";
                var command = new SqlCommand(filterCommand, connection);

                var result = command.ExecuteReader();
                return result.HasRows;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete(Guid apartamentId)
        {
            try
            {
                var connection = new SqlConnection(_connectionString);
                connection.Open();

                var deleteCommand = $"DELETE FROM Apartaments WHERE Id = '{apartamentId}'";
                var command = new SqlCommand(deleteCommand, connection);

                return command.ExecuteNonQuery() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<Apartment> GetAll(int roomsCount)
        {
            var apartaments = new List<Apartment>();

            try
            {
                var connection = new SqlConnection(_connectionString);
                connection.Open();

                string filterCommand = $"SELECT * FROM Apartaments WHERE Rooms_Count = {roomsCount}";
                var command = new SqlCommand(filterCommand, connection);

                var dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        var id = dataReader["Id"];
                        var roomCount = dataReader["Rooms_Count"];

                        var apartament = new Apartment((Guid)id, (int)roomCount);

                        apartaments.Add(apartament);
                    }

                    return apartaments;
                }

                return apartaments;
            }
            catch (Exception)
            {
                return apartaments;
            }
        }

        public bool TryFind(int roomsCount, out Guid apartamentId)
        {
            apartamentId = Guid.Empty;

            try
            {
                var connection = new SqlConnection(_connectionString);
                connection.Open();

                var filterCommand = $"SELECT * FROM Apartaments WHERE Rooms_Count = {roomsCount}";
                var command = new SqlCommand(filterCommand, connection);

                var dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        apartamentId = (Guid)dataReader["Id"];
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
    }
}

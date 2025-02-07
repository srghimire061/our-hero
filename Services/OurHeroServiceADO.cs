using Microsoft.Data.SqlClient;
using OurHeroWebAPI.Models;

namespace OurHeroWebAPI.Services
{
    public class OurHeroServiceADO : IDatabaseService
    {
        private readonly string _connectionString;

        //public OurHeroServiceADO(IConfiguration configuration)
        //{
        //    _connectionString = configuration.GetConnectionString("DefaultConnection");
        //}
        public OurHeroServiceADO(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<OurHero>> GetAllHeros(bool? isActive = null)
        {
            var heros = new List<OurHero>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT * FROM OurHeros WHERE (@isActive IS NULL OR IsActive = @isActive)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@isActive", isActive.HasValue ? (object)isActive.Value : DBNull.Value);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var hero = new OurHero
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                isActive = reader.GetBoolean(reader.GetOrdinal("isActive"))
                            };
                            heros.Add(hero);
                        }
                    }
                }
            }

            return heros;
        }

        public async Task<OurHero> GetHerosByID(int id)
        {
            OurHero hero = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT * FROM OurHeros WHERE Id = @Id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            hero = new OurHero
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                isActive = reader.GetBoolean(reader.GetOrdinal("isActive"))
                            };
                        }
                    }
                }
            }

            return hero;
        }

        public async Task<OurHero> AddOurHero(AddUpdateOurHero heroObject)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "INSERT INTO OurHeros (FirstName, LastName, isActive) OUTPUT INSERTED.Id VALUES (@FirstName, @LastName, @isActive)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", heroObject.FirstName);
                    command.Parameters.AddWithValue("@LastName", heroObject.LastName);
                    command.Parameters.AddWithValue("@isActive", heroObject.isActive);

                    var insertedId = await command.ExecuteScalarAsync();
                    if (insertedId == null)
                    {
                        return null; // Error inserting hero.
                    }

                    // Return the newly created hero with the ID
                    return new OurHero
                    {
                        Id = (int)insertedId,
                        FirstName = heroObject.FirstName,
                        LastName = heroObject.LastName,
                        isActive = heroObject.isActive
                    };
                }
            }
        }

        public async Task<OurHero> UpdateOurHero(int id, AddUpdateOurHero heroObject)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "UPDATE OurHeros SET FirstName = @FirstName, LastName = @LastName, isActive = @isActive WHERE Id = @Id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@FirstName", heroObject.FirstName);
                    command.Parameters.AddWithValue("@LastName", heroObject.LastName);
                    command.Parameters.AddWithValue("@isActive", heroObject.isActive);

                    var rowsAffected = await command.ExecuteNonQueryAsync();
                    if (rowsAffected == 0)
                    {
                        return null; // Hero not found.
                    }

                    // Return the updated hero
                    return new OurHero
                    {
                        Id = id,
                        FirstName = heroObject.FirstName,
                        LastName = heroObject.LastName,
                        isActive = heroObject.isActive
                    };
                }
            }
        }

        public async Task<bool> DeleteHerosByID(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "DELETE FROM OurHeros WHERE Id = @Id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    var rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0; // If no rows were deleted, the hero doesn't exist.
                }
            }
        }
    }
}

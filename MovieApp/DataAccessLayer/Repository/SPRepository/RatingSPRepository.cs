using Common.Constants;
using DataAccessLayer.Repositories.Interface;
using Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DataAccessLayer.Repositories.SPRepository
{
    public class RatingSPRepository : IRatingRepository
    {
        private readonly string _connectionstring;
        private readonly IConfiguration _configuration;

        public RatingSPRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionstring = _configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        public async Task AddRating(Rating rating)
        {
            using (var _connection = new SqlConnection(_connectionstring))
            {
                SqlCommand command = new SqlCommand(StoredProcedureName.RatingInsert, _connection);
                command.CommandType = CommandType.StoredProcedure;

                //parameters
                command.Parameters.AddWithValue("@Id", rating.Id);
                command.Parameters.AddWithValue("@rating", rating.rating);
                command.Parameters.AddWithValue("@RatedBy", rating.RatedBy);
                command.Parameters.AddWithValue("@MovieId", rating.MovieId);
                command.Parameters.AddWithValue("@RatedDate", rating.RatedDate);

                _connection.Open();
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<bool> HasRated(Guid MovieId, string? UserId)
        {
            bool hasRated = false;
            using (var _connection = new SqlConnection(_connectionstring))
            {
                SqlCommand command = new SqlCommand(StoredProcedureName.HasRated, _connection);
                command.CommandType = CommandType.StoredProcedure;

                //Parameters
                command.Parameters.AddWithValue("@MovieId", MovieId);
                command.Parameters.AddWithValue("@UserId", UserId);

                //
                _connection.Open();
                object? result = await command.ExecuteScalarAsync();

                // Check if the result is not null and convert it to a boolean value
                if (result != null && result != DBNull.Value)
                {
                    hasRated = Convert.ToBoolean(result);
                }
            }
            return hasRated;
        }

        public async Task<int> RatingCountByMovie(Guid MovieId)
        {
            using (var _connection = new SqlConnection(_connectionstring))
            {
                SqlCommand command = new SqlCommand(StoredProcedureName.RatingCountByMovie, _connection);
                command.CommandType = CommandType.StoredProcedure;

                //Parameters
                command.Parameters.AddWithValue("@MovieId", MovieId);

                //
                _connection.Open();
                object? result = await command.ExecuteScalarAsync();
                int? count = result as int?;


                return count ?? 0;
            }
        }

        public async Task UpdateRating(Rating rating)
        {
            using (var _connection = new SqlConnection(_connectionstring))
            {
                SqlCommand command = new SqlCommand(StoredProcedureName.RatingUpdate, _connection);
                command.CommandType = CommandType.StoredProcedure;

                // Parameters
                command.Parameters.AddWithValue("@Id", rating.Id);
                command.Parameters.AddWithValue("@rating", rating.rating);
                command.Parameters.AddWithValue("@RatedDate", rating.RatedDate);

                _connection.Open();

                await command.ExecuteNonQueryAsync();
            }
        }
    }
}

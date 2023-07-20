using Common.Constants;
using Common.ViewModels;
using DataAccessLayer.Repositories.Interface;
using Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DataAccessLayer.Repositories.SPRepository
{
    public class DiscussionSPRepository : IDiscussionRepository
    {
        private readonly string _connectionstring;
        private readonly IConfiguration _configuration;

        public DiscussionSPRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionstring= _configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        public async Task AddDiscussion(Discussion discussion)
        {
            using (var _connection = new SqlConnection(_connectionstring))
            {
                SqlCommand command = new SqlCommand(StoredProcedureName.DiscussionInsert, _connection);
                command.CommandType = CommandType.StoredProcedure;

                //parameters
                command.Parameters.AddWithValue("@Id", discussion.Id);
                command.Parameters.AddWithValue("@DiscussionText", discussion.DiscussionText);
                command.Parameters.AddWithValue("@MovieId", discussion.MovieId);
                command.Parameters.AddWithValue("@UserId", discussion.UserId);
                command.Parameters.AddWithValue("@Created", discussion.Created);

                _connection.Open();
                await command.ExecuteNonQueryAsync();
            }

        }

        public async Task DeleteDiscussion(Guid DiscussionId)
        {
            using (var _connection = new SqlConnection(_connectionstring))
            {
                SqlCommand command = new SqlCommand(StoredProcedureName.DiscussionDelete, _connection);
                command.CommandType = CommandType.StoredProcedure;

                //Parameters
                command.Parameters.AddWithValue("@Id", DiscussionId);

                //
                _connection.Open();
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<IEnumerable<DiscussionViewModel>> GetDiscussion(Guid MovieId, int skipcount, int size)
        {
            List<DiscussionViewModel> discussions = new List<DiscussionViewModel>();
            using (var _connection = new SqlConnection(_connectionstring))
            {
                SqlCommand command = new SqlCommand(StoredProcedureName.GetDiscussionForMovie, _connection);
                command.CommandType = CommandType.StoredProcedure;

                //parameters Required
                command.Parameters.AddWithValue("@MovieId", MovieId);
                command.Parameters.AddWithValue("@skipcount", skipcount);
                command.Parameters.AddWithValue("@size", size);

                _connection.Open();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        DiscussionViewModel discussion = new()
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("Id")),
                            DiscussionText = reader.GetString(reader.GetOrdinal("DiscussionText")),
                            UserName = reader.GetString(reader.GetOrdinal("UserName")),
                            Created = reader.GetDateTime(reader.GetOrdinal("Created"))
                        };
                        discussions.Add(discussion);
                    }
                }
            }
            return discussions;
        }

        public async Task<DiscussionViewModel> GetDiscussionById(Guid DiscussionId)
        {
            DiscussionViewModel discussion = new();
            using (var _connection = new SqlConnection(_connectionstring))
            {
                SqlCommand command = new SqlCommand(StoredProcedureName.GetDiscussionById, _connection);
                command.CommandType = CommandType.StoredProcedure;

                //parameters Required
                command.Parameters.AddWithValue("@Id", DiscussionId);

                _connection.Open();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if(reader.Read())
                    {
                        discussion.Id = reader.GetGuid(reader.GetOrdinal("Id"));
                        discussion.DiscussionText = reader.GetString(reader.GetOrdinal("DiscussionText"));
                        discussion.UserName = reader.GetString(reader.GetOrdinal("UserName"));
                        discussion.Created = reader.GetDateTime(reader.GetOrdinal("Created"));
                    }
                }
            }
            return discussion;
        }

        public async Task<int> GetDiscussionCountByMovieId(Guid MovieId)
        {
            using (var _connection = new SqlConnection(_connectionstring))
            {
                SqlCommand command = new SqlCommand(StoredProcedureName.DiscussionCount, _connection);
                command.CommandType = CommandType.StoredProcedure;

                //Parameters
                command.Parameters.AddWithValue("@MovieId", MovieId);

                _connection.Open();
                object? result = await command.ExecuteScalarAsync();
                int? count = result as int?;

                return count ?? 0;
            }
        }
    }
}

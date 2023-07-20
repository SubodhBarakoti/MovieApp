using Common.Constants;
using Common.ViewModels;
using DataAccessLayer.Repositories.Interface;
using Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DataAccessLayer.Repositories.SPRepository
{
    public class GenreSPRepository : IGenreRepository
    {
        private readonly string _connectionstring;
        private readonly IConfiguration _configuration;

        public GenreSPRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionstring = _configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        public async Task AddGenre(Genres genre)
        {
            using (var _connection = new SqlConnection(_connectionstring))
            {
                SqlCommand command = new SqlCommand(StoredProcedureName.GenreInsert, _connection);
                command.CommandType = CommandType.StoredProcedure;

                //Add Parameters
                command.Parameters.AddWithValue("@Id", genre.Id);
                command.Parameters.AddWithValue("@Name", genre.Name);
                command.Parameters.AddWithValue("@Added_By", genre.Added_By);
                command.Parameters.AddWithValue("@Created_Date", genre.Created_Date);

                //connection open and execure
                _connection.Open();
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteGenre(Guid id)
        {
            using (var _connection = new SqlConnection(_connectionstring))
            {

                SqlCommand command = new SqlCommand(StoredProcedureName.GenreDelete, _connection);
                command.CommandType = CommandType.StoredProcedure;

                //Parameters
                command.Parameters.AddWithValue("@Id", id);

                //
                _connection.Open();
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<IEnumerable<GenreViewModel>> GetAllGenres()
        {
            List<GenreViewModel> genres = new List<GenreViewModel>();
            using (var _connection = new SqlConnection(_connectionstring))
            {
                SqlCommand command = new SqlCommand(StoredProcedureName.GetAllGenres, _connection);
                command.CommandType = CommandType.StoredProcedure;

                //No parameters Required

                _connection.Open();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        GenreViewModel genre = new()
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };
                        genres.Add(genre);
                    }
                }
            }
            return genres;
        }

        public async Task<GenreViewModel> GetGenreById(Guid id)
        {
            GenreViewModel genre= new GenreViewModel();
            using (var _connection = new SqlConnection(_connectionstring))
            {
                SqlCommand command = new SqlCommand(StoredProcedureName.GetGenreById, _connection);
                command.CommandType = CommandType.StoredProcedure;

                //Parameters
                command.Parameters.AddWithValue("@Id", id);

                _connection.Open();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        genre.Id = reader.GetGuid(reader.GetOrdinal("Id"));
                        genre.Name = reader.GetString(reader.GetOrdinal("Name"));
                    }
                }
            }
            return genre;
        }

        public async Task UpdateGenre(Genres genre)
        {
            // add sp for this as well
            using (var _connection = new SqlConnection(_connectionstring))
            {
                SqlCommand command = new SqlCommand(StoredProcedureName.GenreUpdate, _connection);
                command.CommandType = CommandType.StoredProcedure;

                //Parameters
                command.Parameters.AddWithValue("@Id", genre.Id);
                command.Parameters.AddWithValue("@Name", genre.Name);

                //
                _connection.Open();
                await command.ExecuteNonQueryAsync();
            }

        }
    }
}

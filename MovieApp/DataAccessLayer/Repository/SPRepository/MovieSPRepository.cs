using Common.Constants;
using Common.ViewModels;
using DataAccessLayer.Repositories.Interface;
using Entities;
using Common.Enums;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace DataAccessLayer.Repositories.SPRepository
{
    public class MovieSPRepository : IMovieRepository
    {
        private readonly string _connectionstring;
        private readonly IConfiguration _configuration;

        public MovieSPRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionstring = _configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        public async Task DeleteMovie(Guid id)
        {
            using (var _connection = new SqlConnection(_connectionstring))
            {
                SqlCommand command = new SqlCommand(StoredProcedureName.MovieDelete, _connection);
                command.CommandType = CommandType.StoredProcedure;

                //Parameters
                command.Parameters.AddWithValue("@Id", id);

                //
                _connection.Open();
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<IEnumerable<MovieViewModel>> GetAllMovies(int skip_count, int page_size, Guid? GenreId, MovieSortBy movieSortBy)
        {
            List<MovieViewModel> movies = new List<MovieViewModel>();
            using (var _connection = new SqlConnection(_connectionstring))
            {
                SqlCommand command = new SqlCommand(StoredProcedureName.GetAllMovies, _connection);
                command.CommandType = CommandType.StoredProcedure;

                //parameters Required
                command.Parameters.AddWithValue("@skip_count", skip_count);
                command.Parameters.AddWithValue("@page_size", page_size);
                command.Parameters.AddWithValue("@GenreId", GenreId);


                // Can I do this??
                command.Parameters.AddWithValue("@movieSortBy", movieSortBy.ToString());







                _connection.Open();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        MovieViewModel movie = new()
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Description = reader.GetString(reader.GetOrdinal("Description")),
                            GenreName = reader.GetString(reader.GetOrdinal("GenreName")),
                            ReleaseDate = reader.GetDateTime(reader.GetOrdinal("ReleaseDate")),
                            MovieDuration = reader.GetString(reader.GetOrdinal("MovieDuration")),
                            ImagePath = reader.GetString(reader.GetOrdinal("ImagePath")),
                            AverageRating = reader.GetDecimal(reader.GetOrdinal("AverageRating"))
                        };
                        movies.Add(movie);
                    }
                }
            }
            return movies;
        }

        public async Task<decimal> GetAverageRating(Guid id)
        {
            using (var _connection = new SqlConnection(_connectionstring))
            {
                SqlCommand command = new SqlCommand(StoredProcedureName.GetAverageRating, _connection);
                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.AddWithValue("@Id", id);


                _connection.Open();

                decimal AverageRating = (decimal)await command.ExecuteScalarAsync();
                return AverageRating;
            }
        }

        public async Task<Movie> GetMovieById(Guid id)
        {
            Movie movie=new Movie();
            using (var _connection = new SqlConnection(_connectionstring))
            {
                _connection.Open();
                SqlCommand command = new SqlCommand(StoredProcedureName.GetMovieById, _connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Id", id);


                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        movie = new()
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Description = reader.GetString(reader.GetOrdinal("Description")),
                            GenreId = reader.GetGuid(reader.GetOrdinal("GenreId")),
                            ReleaseDate = reader.GetDateTime(reader.GetOrdinal("ReleaseDate")),
                            MovieDuration = reader.GetString(reader.GetOrdinal("MovieDuration")),
                            ImagePath = reader.GetString(reader.GetOrdinal("ImagePath")),
                            AverageRating = reader.GetDecimal(reader.GetOrdinal("AverageRating"))
                        };
                    }
                }
            }
            return movie;
        }

        public async Task<MovieViewModel> GetViewMovieById(Guid id)
        {
            using (var _connection = new SqlConnection(_connectionstring))
            {
                SqlCommand command = new SqlCommand(StoredProcedureName.GetViewMovieById, _connection);
                command.CommandType = CommandType.StoredProcedure;

                //parameters
                command.Parameters.AddWithValue("@Id", id);

                _connection.Open();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new MovieViewModel()
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Description = reader.GetString(reader.GetOrdinal("Description")),
                            GenreName = reader.GetString(reader.GetOrdinal("GenreName")),
                            ReleaseDate = reader.GetDateTime(reader.GetOrdinal("ReleaseDate")),
                            MovieDuration = reader.GetString(reader.GetOrdinal("MovieDuration")),
                            ImagePath = reader.GetString(reader.GetOrdinal("ImagePath")),
                            AverageRating = reader.GetDecimal(reader.GetOrdinal("AverageRating"))
                        };
                    }
                }
            }
            return null;
        }
        
        public async Task InsertMovie(Movie movie)
        {
            using (var _connection = new SqlConnection(_connectionstring))
            {
                //Configure Command
                SqlCommand command = new SqlCommand(StoredProcedureName.MovieInsert, _connection);
                command.CommandType = CommandType.StoredProcedure;

                //Setting Parameters
                command.Parameters.AddWithValue("@Id", movie.Id);
                command.Parameters.AddWithValue("@Name", movie.Name);
                command.Parameters.AddWithValue("@Description", movie.Description);
                command.Parameters.AddWithValue("@MovieDuration", movie.MovieDuration);
                command.Parameters.AddWithValue("@GenreId", movie.GenreId);
                command.Parameters.AddWithValue("@AddedBy", movie.added_by);
                command.Parameters.AddWithValue("@AddedDate", movie.AddedDate);
                command.Parameters.AddWithValue("@ReleaseDate", movie.ReleaseDate);
                command.Parameters.AddWithValue("@ImagePath", movie.ImagePath);

                _connection.Open();
                await command.ExecuteNonQueryAsync();
            }
        }
        
        public async Task<int> MovieCountByGenre(Guid? GenreId)
        {
            //check if the genre is provided or not in the database
            using (var _connection = new SqlConnection(_connectionstring))
            {
                SqlCommand command = new SqlCommand(StoredProcedureName.GetMovieCount, _connection);
                command.CommandType = CommandType.StoredProcedure;


                // how to put null value to the database??
                // Ask Prashant
                // parameters
                command.Parameters.AddWithValue("@GenreId", GenreId ?? (object)DBNull.Value);
                _connection.Open();

                int count = (int)await command.ExecuteScalarAsync();

                return count;
            }
        }

        public async Task UpdateImage(Guid MovieId, string ImagePath)
        {
            using(var _connection = new SqlConnection(_connectionstring))
            {
                SqlCommand command = new SqlCommand(StoredProcedureName.UpdateMovieImage, _connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@MovieId",MovieId);
                command.Parameters.AddWithValue("@ImagePath",ImagePath);

                _connection.Open();
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task UpdateMovie(Movie movie)
        {
            using (var _connection = new SqlConnection(_connectionstring))
            {
                //Configure Command
                SqlCommand command = new SqlCommand(StoredProcedureName.MovieUpdate, _connection);
                command.CommandType = CommandType.StoredProcedure;

                //Setting Parameters
                command.Parameters.AddWithValue("@Id", movie.Id);
                command.Parameters.AddWithValue("@Name", movie.Name);
                command.Parameters.AddWithValue("@Description", movie.Description);
                command.Parameters.AddWithValue("@MovieDuration", movie.MovieDuration);
                command.Parameters.AddWithValue("@GenreId", movie.GenreId);
                command.Parameters.AddWithValue("@ReleaseDate", movie.ReleaseDate);

                _connection.Open();
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}

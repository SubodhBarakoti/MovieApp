using Common.Constants;
using Common.Enums;
using Common.ViewModels;
using Dapper;
using DataAccessLayer.Persistance.Dapper;
using DataAccessLayer.Repositories.Interface;
using Entities;
using System.Data;

namespace DataAccessLayer.Repository.DapperRepository
{
    public class DapperMovieRepository : IMovieRepository
    {
        public readonly DapperContext _dapperContext;

        public DapperMovieRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task DeleteMovie(Guid id)
        {
            using(var connection = _dapperContext.CreateConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);
                await connection.ExecuteAsync(StoredProcedureName.MovieDelete, parameters,commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<MovieViewModel>> GetAllMovies(int skip_count, int page_size, Guid? GenreId, MovieSortBy movieSortBy)
        {
            using(var connection = _dapperContext.CreateConnection())
            {
                var parameters = new DynamicParameters();

                
                parameters.Add("@skip_count", skip_count);
                parameters.Add("@page_size", page_size);
                parameters.Add("@GenreId", GenreId);
                parameters.Add("@movieSortBy", movieSortBy.ToString());

                var movies = await connection.QueryAsync<MovieViewModel>(StoredProcedureName.GetAllMovies,parameters,commandType:CommandType.StoredProcedure);
                return movies;
            }
        }

        public async Task<decimal> GetAverageRating(Guid id)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                var parameters = new DynamicParameters();

                parameters.Add("@MovieId", id);

                var average= await connection.QueryFirstOrDefaultAsync<decimal>(StoredProcedureName.GetAverageRating, parameters,commandType: CommandType.StoredProcedure);

                return average;
            }
        }

        public async Task<Movie> GetMovieById(Guid id)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                var parameters = new DynamicParameters();

                parameters.Add("@Id", id);

                var movies = await connection.QueryFirstOrDefaultAsync<Movie>(StoredProcedureName.GetMovieById, parameters, commandType: CommandType.StoredProcedure);
                return movies;
            }
        }

        public async Task<MovieViewModel> GetViewMovieById(Guid id)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                var parameters = new DynamicParameters();

                parameters.Add("@Id", id);

                var movie = await connection.QueryFirstOrDefaultAsync<MovieViewModel>(StoredProcedureName.GetViewMovieById, parameters,commandType: CommandType.StoredProcedure);

                return movie;
            }
        }

        public async Task InsertMovie(Movie movie)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                var parameters = new DynamicParameters();

                parameters.Add("@Id", movie.Id);
                parameters.Add("@Name", movie.Name);
                parameters.Add("@Description", movie.Description);
                parameters.Add("@MovieDuration", movie.MovieDuration);
                parameters.Add("@GenreId", movie.GenreId);
                parameters.Add("@AddedBy", movie.added_by);
                parameters.Add("@AddedDate", movie.AddedDate);
                parameters.Add("@ReleaseDate", movie.ReleaseDate);
                parameters.Add("@ImagePath", movie.ImagePath);

                await connection.ExecuteAsync(StoredProcedureName.MovieInsert, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> MovieCountByGenre(Guid? GenreId)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                var parameters = new DynamicParameters();

                parameters.Add("@GenreId", GenreId);
                var count = await connection.QueryFirstOrDefaultAsync<int>(StoredProcedureName.GetMovieCount, parameters, commandType: CommandType.StoredProcedure);
                return count;
            }
        }

        public async Task UpdateImage(Guid MovieId, string ImagePath)
        {
            using(var connection =  _dapperContext.CreateConnection())
            {
                var parameters = new DynamicParameters();

                parameters.Add("@MovieId",MovieId);
                parameters.Add("@ImagePath",ImagePath);

                await connection.ExecuteAsync(StoredProcedureName.UpdateMovieImage,parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task UpdateMovie(Movie movie)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                var parameters = new DynamicParameters();

                parameters.Add("@Id", movie.Id);
                parameters.Add("@Name", movie.Name);
                parameters.Add("@Description", movie.Description);
                parameters.Add("@MovieDuration", movie.MovieDuration);
                parameters.Add("@GenreId", movie.GenreId);
                parameters.Add("@ReleaseDate", movie.ReleaseDate);

                await connection.ExecuteAsync(StoredProcedureName.MovieUpdate, parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}

using Common.Enums;
using Common.ViewModels;
using Entities;

namespace DataAccessLayer.Repositories.Interface
{
    public interface IMovieRepository
    {
        Task DeleteMovie(Guid id);
        Task<Movie> GetMovieById(Guid id);
        Task InsertMovie(Movie movie);
        Task UpdateMovie(Movie movie);
        Task<IEnumerable<MovieViewModel>> GetAllMovies(int skip_count, int page_size, Guid? GenreId, MovieSortBy movieSortBy);
        Task<decimal> GetAverageRating(Guid id);
        Task<MovieViewModel> GetViewMovieById(Guid id);
        Task<int> MovieCountByGenre(Guid? GenreId);
        Task UpdateImage(Guid MovieId, string ImagePath);
    }
}
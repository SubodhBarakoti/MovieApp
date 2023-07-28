using Common.Enums;
using Common.ViewModels;
using Entities;

namespace Services.Interface
{
    public interface IMovieService
    {
        Task CalculateAverageRating(int count, int rate, Guid MovieId);
        Task ChangeImage(ChangeImageViewModel uimage);
        Task DeleteMovie(Guid id);
        Task<DisplayMoviesViewModel> GetAllMovies(Guid? GenreId, MovieSortBy sortBy, int pageno);
        Task<decimal> GetAverageRating(Guid id);
        Task<UpdateMovieViewModel> GetEditItemById(Guid id);
        Task<Movie> GetMovieById(Guid id);
        Task<MovieViewModel> GetViewMovieById(Guid id);
        Task InsertMovie(InsertMovieViewModel imovie,string UserId);
        Task<dynamic> SendEmail(Guid MovieId, string Email);
        Task UpdateMovie(Guid id, UpdateMovieViewModel umovie);
    }
}
using Common.ViewModels;
using Entities;
namespace DataAccessLayer.Repositories.Interface
{
    public interface IGenreRepository
    {
        Task AddGenre(Genres genre);
        Task DeleteGenre(Guid id);
        Task<IEnumerable<GenreViewModel>> GetAllGenres();
        Task<GenreViewModel> GetGenreById(Guid id);
        Task UpdateGenre(Genres genre);
    }
}
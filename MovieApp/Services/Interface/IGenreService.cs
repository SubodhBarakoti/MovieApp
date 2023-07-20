using Common.ViewModels;
using Entities;

namespace Services.Interface
{
    public interface IGenreService
    {
        Task AddGenre(string genre_name, string UserId);
        Task DeleteGenre(Guid id);
        Task<IEnumerable<GenreViewModel>> GetAllGenre();
        Task<GenreViewModel> GetGenreById(Guid id);
        Task UpdateGenre(Guid GenreId, string GenreName);
    }
}
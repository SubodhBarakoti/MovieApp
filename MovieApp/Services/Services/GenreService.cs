using Common.ViewModels;
using DataAccessLayer.Repositories;
using Entities;
using Services.Interface;

namespace Services.Services
{
    public class GenreService : IGenreService
    {
        private readonly IAllRepositories _repository;

        public GenreService(IAllRepositories repository)
        {
            _repository = repository;
        }
        public async Task AddGenre(string genre_name, string AddedBy)
        {
            Genres genre = new()
            {
                Id = Guid.NewGuid(),
                Name = genre_name,
                Added_By = AddedBy,
                Created_Date = DateTime.Now
            };
            await _repository.Genre.AddGenre(genre);
        }
        public async Task<GenreViewModel> GetGenreById(Guid id)
        {
            return await _repository.Genre.GetGenreById(id);
        }
        public async Task<IEnumerable<GenreViewModel>> GetAllGenre()
        {
            return await _repository.Genre.GetAllGenres();
        }
        public async Task DeleteGenre(Guid id)
        {
            if(await _repository.Genre.GetGenreById(id) != null)
            {
                await _repository.Genre.DeleteGenre(id);
            }
        }
        public async Task UpdateGenre(Guid GenreId, string GenreName)
        {
            if (await _repository.Genre.GetGenreById(GenreId) != null)
            {
                Genres genre = new()
                {
                    Id = GenreId,
                    Name = GenreName
                };
                await _repository.Genre.UpdateGenre(genre);
            }
        }
    }
}

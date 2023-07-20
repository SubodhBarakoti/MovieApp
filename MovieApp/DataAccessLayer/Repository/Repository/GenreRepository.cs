using Common.ViewModels;
using DataAccessLayer.Persistance;
using DataAccessLayer.Repositories.Interface;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories.Repository
{
    public class GenreRepository : IGenreRepository
    {
        private readonly ApplicationDbContext _context;

        public GenreRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddGenre(Genres genre)
        {
            await _context.tbl_Genres.AddAsync(genre);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<GenreViewModel>> GetAllGenres()
        {
            return await _context.tbl_Genres.Select(g => new GenreViewModel
            {
                Id = g.Id,
                Name = g.Name
            }).ToListAsync();
        }
        public async Task DeleteGenre(Guid id)
        {
            var genre = await _context.tbl_Genres.FindAsync(id);
            if (genre != null)
            {
                _context.tbl_Genres.Remove(genre);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<GenreViewModel> GetGenreById(Guid id)
        {
            
            return await _context.tbl_Genres
                .Select(g=> new GenreViewModel
                    {
                        Id= g.Id,
                        Name= g.Name
                    })
                .Where(g=>g.Id == id).FirstOrDefaultAsync();
        }

        public async Task UpdateGenre(Genres genre)
        {
            _context.Entry(genre).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}

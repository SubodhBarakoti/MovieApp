using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Persistance;
using DataAccessLayer.Repositories.Interface;
using Entities;

namespace DataAccessLayer.Repositories.Repository
{
    public class RatingRepository : IRatingRepository
    {
        private readonly ApplicationDbContext _context;
        public RatingRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddRating(Rating rating)
        {
            await _context.tbl_Ratings.AddAsync(rating);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateRating(Rating rating)
        {
            _context.Entry(rating).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task<int> RatingCountByMovie(Guid MovieId)
        {
            return await _context.tbl_Ratings.Where(m => m.MovieId == MovieId).CountAsync();
        }
        public async Task<bool> HasRated(Guid MovieId, string UserId)
        {
            return await _context.tbl_Ratings.AnyAsync(r => r.MovieId == MovieId && r.RatedBy == UserId);
        }
    }
}

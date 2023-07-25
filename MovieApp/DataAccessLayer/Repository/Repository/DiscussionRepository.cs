using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Persistance;
using DataAccessLayer.Repositories.Interface;
using Common.ViewModels;
using Entities;

namespace DataAccessLayer.Repositories.Repository
{
    public class DiscussionRepository : IDiscussionRepository
    {
        private readonly ApplicationDbContext _context;

        public DiscussionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddDiscussion(Discussion discussion)
        {
            await _context.tbl_Discussions.AddAsync(discussion);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<DiscussionViewModel>> GetDiscussion(Guid MovieId, int skipcount, int size)
        {
            return await _context.tbl_Discussions.Where(d => d.MovieId == MovieId)
                .Select(d => new DiscussionViewModel
                {
                    Id = d.Id,
                    DiscussionText = d.DiscussionText,
                    UserName = d.User.UserName,
                    Created = d.Created
                })
                .Skip(skipcount)
                .Take(size)
                .OrderByDescending(d => d.Created)
                .ToListAsync();
        }
        public async Task<int> GetDiscussionCountByMovieId(Guid MovieId)
        {
            return await _context.tbl_Discussions.Where(d => d.MovieId == MovieId).CountAsync();
        }
        public async Task DeleteDiscussion(Guid DiscussionId)
        {
            var discussion = await _context.tbl_Discussions.FindAsync(DiscussionId);
            if (discussion != null)
            {
                _context.tbl_Discussions.Remove(discussion);
            }
            await _context.SaveChangesAsync();
        }

        

        public async Task<DiscussionViewModel?> GetDiscussionById(Guid DiscussionId)
        {
            return await _context.tbl_Discussions.Select(d => new DiscussionViewModel
            {
                Id = d.Id,
                DiscussionText = d.DiscussionText,
                Created = d.Created,
                UserName = d.User.UserName
            }).Where(d => d.Id == DiscussionId).FirstOrDefaultAsync();
        }
    }
}

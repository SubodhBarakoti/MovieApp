using Common.ViewModels;
using Entities;

namespace DataAccessLayer.Repositories.Interface
{
    public interface IDiscussionRepository
    {
        Task AddDiscussion(Discussion discussion);
        Task DeleteDiscussion(Guid DiscussionId);
        Task<IEnumerable<DiscussionViewModel>> GetDiscussion(Guid MovieId, int skipcount, int size);
        Task<int> GetDiscussionCountByMovieId(Guid MovieId);
        Task<DiscussionViewModel> GetDiscussionById(Guid DiscussionId);
    }
}
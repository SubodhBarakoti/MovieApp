using Common.ViewModels;

namespace Services.Interface
{
    public interface IDiscussionService
    {
        Task AddDiscussion(AddDiscussionViewModel discuss, string UserId);
        Task DeleteDiscussionById(Guid DiscussionId);
        Task<DiscussionsViewModel> GetDiscussionsByMovie(Guid MovieId, int page);
        Task<DiscussionViewModel> GetDiscussionById(Guid DiscussionId);
    }
}
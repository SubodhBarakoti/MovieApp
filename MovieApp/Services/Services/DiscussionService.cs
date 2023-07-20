using Common.ViewModels;
using DataAccessLayer.Repositories;
using Entities;
using Services.Interface;

namespace Services.Services
{
    public class DiscussionService : IDiscussionService
    {
        private readonly IAllRepositories _repository;
        private readonly IPagerService _pager;

        public DiscussionService(IAllRepositories repository, IPagerService pager)
        {
            _repository = repository;
            _pager = pager;
        }
        public async Task AddDiscussion(AddDiscussionViewModel discuss, string UserId)
        {
            Discussion discussion = new()
            {
                Id = Guid.NewGuid(),
                DiscussionText = discuss.DiscussionText,
                MovieId = discuss.MovieId,
                UserId = UserId,
                Created = DateTime.Now
            };
            await _repository.Discussion.AddDiscussion(discussion);
        }
        public async Task<DiscussionsViewModel> GetDiscussionsByMovie(Guid MovieId, int page)
        {
            int size = 5;
            int discussioncount = await _repository.Discussion.GetDiscussionCountByMovieId(MovieId);
            _pager.PagerInitialize(discussioncount, page, size);
            int skipcount = (_pager.CurrentPage - 1) * _pager.PageSize;
            var Discussions = await _repository.Discussion.GetDiscussion(MovieId, skipcount, size);
            PagerViewModel discussionpage = new()
            {
                CurrentPage = _pager.CurrentPage,
                TotalPages = _pager.TotalPages,
                StartPage = _pager.StartPage,
                EndPage = _pager.EndPage,
            };
            DiscussionsViewModel discussionsViewModel = new()
            {
                Discussions = Discussions,
                pager = discussionpage
            };
            return discussionsViewModel;
        }
        public async Task DeleteDiscussionById(Guid DiscussionId)
        {
            await _repository.Discussion.DeleteDiscussion(DiscussionId);
        }

        public async Task<DiscussionViewModel> GetDiscussionById(Guid DiscussionId)
        {
            return await _repository.Discussion.GetDiscussionById(DiscussionId);
        }
    }
}

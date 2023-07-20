using Common.ViewModels;
using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace MovieAppMVC.Controllers
{
    public class DiscussionController : Controller
    {
        private readonly UserManager<Users> _userManager;
        private readonly IDiscussionService _discussionService;

        public DiscussionController
            (
                UserManager<Users> userManager,
                IDiscussionService discussionService
            )
        {
            _userManager = userManager;
            _discussionService = discussionService;
        }
        [HttpPost]
        public async Task<IActionResult> AddDiscussion([Bind("MovieId,DiscussionText")] AddDiscussionViewModel discussion)
        {
            var UserId = _userManager.GetUserId(User) ?? throw new ArgumentNullException(nameof(User));
            await _discussionService.AddDiscussion(discussion, UserId);
            return RedirectToAction("ViewDiscussion", new {MovieId=discussion.MovieId});
        }
        [HttpGet]
        public async Task<IActionResult> ViewDiscussion(Guid MovieId,int page=1)
        {
            var discussion = await _discussionService.GetDiscussionsByMovie(MovieId, page);
            return PartialView("_GetDiscussion",discussion);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteDiscussion(Guid MovieId,Guid DiscussionId)
        {
            await _discussionService.DeleteDiscussionById(DiscussionId);
            return RedirectToAction("ViewDiscussion", new { MovieId = MovieId });
        }
    }
}

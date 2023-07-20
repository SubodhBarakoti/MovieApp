using Common.ViewModels;
using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace MovMovieAppMVCieApp.Controllers
{
    public class RatingController : Controller
    {
        private readonly UserManager<Users> _userManager;
        private readonly IMovieService _movieService;
        private readonly IRatingService _ratingService;

        public RatingController
            (
                UserManager<Users> userManager,
                IMovieService movieService,
                IRatingService ratingService
            )
        {
            _userManager = userManager;
            _movieService = movieService;
            _ratingService = ratingService;
        }
        [HttpPost]
        public async Task<IActionResult> SubmitRating([Bind("MovieId,rating")] RateViewModel rate)
        {
            var RatedBy = _userManager.GetUserId(User) ?? throw new ArgumentNullException(nameof(User));
            await _ratingService.AddRating(rate, RatedBy);
            var averagerating=await _movieService.GetAverageRating(rate.MovieId);
            return Content(averagerating.ToString());
        }
    }
}

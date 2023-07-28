using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Services.Interface;
using Common.Enums;
using Common.ViewModels;
using Entities;

namespace MovieAppMVC.Controllers
{
    //[Authorize(Roles = "Admin,Super_Admin,Basic user")]
    public class MoviesController : Controller
    {
        private readonly UserManager<Users> _userManager;
        private readonly IMovieService _movieService;
        private readonly IDiscussionService _discussionService;
        private readonly IRatingService _ratingService;
        private readonly IGenreService _genreService;
        private readonly IConfiguration _configuration;
        private readonly string ImageLocation;

        public MoviesController
            (
                UserManager<Users> userManager,
                IMovieService movieService,
                IDiscussionService discussionService,
                IRatingService ratingService,
                IGenreService genreService,
                IConfiguration configuration)
        {
            _userManager = userManager;
            _movieService = movieService;
            _discussionService = discussionService;
            _ratingService = ratingService;
            _genreService = genreService;
            _configuration = configuration;
            ImageLocation = _configuration.GetValue<string>("ProjectLocation")?? throw new InvalidOperationException("Not found Project Location");
        }
        [HttpGet]
        // GET: Movies
        public async Task<IActionResult> Index(Guid? genre=null, MovieSortBy sortBy = MovieSortBy.Name, int page = 1)
        {
            var filteredMovies = await _movieService.GetAllMovies(genre, sortBy, page);
            ViewData["GenreId"] = new SelectList(await _genreService.GetAllGenre(), "Id", "Name");
            ViewData["ImageLocation"] = ImageLocation;
            return View(filteredMovies);
        }
        public async Task<IActionResult> BrowseMovies(Guid? genre=null, MovieSortBy sortBy = MovieSortBy.Name, int page = 1)
        {
            var filteredMovies = await _movieService.GetAllMovies(genre, sortBy, page);
            ViewData["ImageLocation"] = ImageLocation;
            return PartialView("_MovieList",filteredMovies);
        }
        [HttpGet]
        public IActionResult FilterMovies(MovieSortBy sortBy, Guid? genre=null,  int page = 1)
        {
            return RedirectToAction("BrowseMovies", new { genre, sortBy, page });
        }
        // GET: Movies/Details/5
        public async Task<IActionResult> Details(Guid id, int page=1)
        {
           
            var movie = await _movieService.GetViewMovieById(id);
            if (movie == null)
            {
                return NotFound();
            }
            var discussions=await _discussionService.GetDiscussionsByMovie(id, page);
            if (discussions == null)
            {
                return NotFound();
            }
            ViewData["ImageLocation"] = ImageLocation;
            MovieDetailsViewModel MovieDiscussion = new()
            {
                Movie = movie,
                Discussions = discussions
            };
            if (User.Identity.IsAuthenticated)
            {
                var userid = _userManager.GetUserId(User) ?? throw new ArgumentNullException(nameof(User));
                ViewBag.HasRated = await _ratingService.HasRated(movie.Id, userid);
            }
            return View(MovieDiscussion);
        }

        // GET: Movies/Create
        [Authorize(Roles = "Admin,Super_Admin")]
        public async Task<IActionResult> CreateAsync()
        {
            ViewData["GenreId"]= new SelectList(await _genreService.GetAllGenre(), "Id", "Name");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Super_Admin")]
        public async Task<IActionResult> Create([Bind("Name,Description,MovieDuration,GenreId,ReleaseDate,ImageFile")] InsertMovieViewModel movie)
        {
            if (ModelState.IsValid)
            {
                var UserId = _userManager.GetUserId(User) ?? throw new ArgumentNullException();
                await _movieService.InsertMovie(movie, UserId);
                TempData["Message"] = "Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            ViewData["GenreId"] = new SelectList(await _genreService.GetAllGenre(), "Id", "Name");
            return View(movie);
        }

        [Authorize(Roles = "Admin,Super_Admin")]
        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var movie = await _movieService.GetEditItemById(id);
            ViewData["GenreId"] = new SelectList(await _genreService.GetAllGenre(), "Id", "Name");
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Super_Admin")]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Description,MovieDuration,GenreId,ReleaseDate")] UpdateMovieViewModel umovie)
        {
            if (ModelState.IsValid)
            {
                await _movieService.UpdateMovie(id, umovie);
                TempData["Message"] = "Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            ViewData["GenreId"] = new SelectList(await _genreService.GetAllGenre(), "Id", "Name");
            return View(umovie);
        }

        [Authorize(Roles = "Admin,Super_Admin")]
        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            var movie = await _movieService.GetViewMovieById(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        [Authorize(Roles = "Admin,Super_Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _movieService.DeleteMovie(id);
            TempData["Message"] = "Deleted Successfully";
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> ChangeMovieImage(Guid id)
        {
            ViewData["ImageLocation"] = ImageLocation;
            var movie = await _movieService.GetViewMovieById(id);
            return View(movie);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeMovieImage([Bind("MovieId,ImageFile")] ChangeImageViewModel cimage)
        {
            await _movieService.ChangeImage(cimage);
            return RedirectToAction("Details", routeValues: new { id = cimage.MovieId });
        }

        [HttpPost]
        public async Task<IActionResult> ShareMovie(Guid MovieId, string Email)
        {
            var response = await _movieService.SendEmail(MovieId, Email);
            return Ok(response);
        }
    }
}
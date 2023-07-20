using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Services.Interface;
using Common.ViewModels;
using Entities;

namespace MovieAppMVC.Controllers
{
    public class GenresController : Controller
    {
        private readonly IGenreService _genreService;
        private readonly UserManager<Users> _userManager;

        public GenresController
            (
            IGenreService genreService,
            UserManager<Users> userManager
            )
        {
            _genreService = genreService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
              return View(await _genreService.GetAllGenre());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] GenreViewModel genres)
        {
            if (ModelState.IsValid)
            {
                string userId = _userManager.GetUserId(User) ?? throw new NullReferenceException();
                await _genreService.AddGenre(genres.Name, userId);
                return RedirectToAction(nameof(Index));
            }
            return View(genres);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var genres = await _genreService.GetGenreById(id);
            if (genres == null)
            {
                return NotFound();
            }

            return View(genres);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            
            await _genreService.DeleteGenre(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

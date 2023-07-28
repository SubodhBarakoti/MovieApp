using Common.Enums;
using Common.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;
using System.Net;
using System.Security.Claims;

namespace MovieAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [Authorize(Roles = nameof(UserRole.Super_Admin) + "," + nameof(UserRole.Admin))]
        [HttpGet]
        [Route("~/api/Movies")]
        public async Task<Response<DisplayMoviesViewModel>> GetAllMovies(Guid? GenreId = null, MovieSortBy sortBy = MovieSortBy.Name, int page = 1)
        {
            try
            {
                var movies = await _movieService.GetAllMovies(GenreId, sortBy, page);
                if (movies.Movies != null && movies.Movies.Count()>=1)
                {
                    return new Response<DisplayMoviesViewModel> { Status = Status.Data.ToString(), Message = "Data Found", HttpStatus = HttpStatusCode.OK, Data = movies };
                }
                return new Response<DisplayMoviesViewModel> { Status = Status.Error.ToString(), HttpStatus = HttpStatusCode.NotFound, Message = "No Movies can be found" };

            }
            catch (Exception ex)
            {
                return new Response<DisplayMoviesViewModel> { Status = Status.Error.ToString(), Message = ex.Message, HttpStatus=HttpStatusCode.InternalServerError };
            }
        }



        //[Authorize(Roles = nameof(UserRole.Super_Admin) + "," + nameof(UserRole.Admin))]
        [HttpGet]
        [Route("~/api/Movies/{MovieId}")]
        public async Task<Response<MovieViewModel>> GetMovieById(Guid MovieId)
        {
            try
            {
                var movie = await _movieService.GetViewMovieById(MovieId);
                if (movie != null)
                {
                    return  new Response<MovieViewModel> { Status = Status.Data.ToString(), Message = "Data Found", HttpStatus=HttpStatusCode.OK, Data = movie };
                }
                return new Response<MovieViewModel> { Status = Status.Warning.ToString(), Message = "No Movies can be found", HttpStatus = HttpStatusCode.NotFound};
            }
            catch (Exception ex)
            {
                return new Response<MovieViewModel> { Status = Status.Error.ToString(), Message = ex.Message, HttpStatus = HttpStatusCode.InternalServerError };
            }
        }



        [Authorize(Roles = nameof(UserRole.Super_Admin) + "," + nameof(UserRole.Admin))]
        [HttpPost]
        [Route("~/api/CreateMovie")]
        public async Task<Response<string>> PostNewMovie([FromForm]InsertMovieViewModel movie)
        {
            try
            {
                var UserId = User.FindFirstValue("UserId");
                await _movieService.InsertMovie(movie, UserId);
                return new Response<string> { Status = Status.Success.ToString(), Message = "Movie Added Successfully.", HttpStatus = HttpStatusCode.OK };
            }
            catch (Exception ex)
            {
                return new Response<string> { Status = Status.Error.ToString(), Message = ex.Message, HttpStatus = HttpStatusCode.InternalServerError };
            }
        }
        


        [Authorize(Roles = nameof(UserRole.Super_Admin) + "," + nameof(UserRole.Admin))]
        [Route("~/api/DeleteMovie/{MovieId}")]
        [HttpDelete]
        public async Task<Response<string>> DeleteMovie(Guid MovieId)
        {
            try
            {
                if( await _movieService.GetMovieById(MovieId) != null )
                {
                    await _movieService.DeleteMovie(MovieId);
                    return new Response<string> { Status = Status.Success.ToString(), Message = "Movie Deleted Successfully",HttpStatus= HttpStatusCode.NoContent };
                }
                return new Response<string> { Status = Status.NotFound.ToString(), Message = "Movie Not Found", HttpStatus = HttpStatusCode.NotFound };
            }
            catch (Exception ex)
            {
                return new Response<string> { Status = Status.Error.ToString(), Message = ex.Message, HttpStatus = HttpStatusCode.InternalServerError };
            }
        }
        


        [Authorize(Roles = nameof(UserRole.Super_Admin) + "," + nameof(UserRole.Admin))]
        [Route("~/api/UpdateMovie")]
        [HttpPut]
        public async Task<Response<string>> UpdateMovie(Guid MovieId, UpdateMovieViewModel update)
        {
            try
            {
                await _movieService.UpdateMovie(MovieId, update);
                return new Response<string> { Status = Status.Success.ToString(), Message = "Movie Updated Successfully", HttpStatus=HttpStatusCode.NoContent };
            }
            catch (Exception ex)
            {
                return new Response<string> { Status = Status.Error.ToString(), Message = ex.Message, HttpStatus=HttpStatusCode.InternalServerError };
            }
        }
        

        
        [Authorize(Roles = nameof(UserRole.Super_Admin) + "," + nameof(UserRole.Admin))]
        [Route("~/api/ChangeImage")]
        [HttpPatch]
        public async Task<Response<string>> ChangeImage([FromForm]ChangeImageViewModel changeimage)
        {
            try
            {
                await _movieService.ChangeImage(changeimage);
                return new Response<string> { Status = Status.Success.ToString(), Message = "Image Changed Successfully" };
            }
            catch (Exception ex)
            {
                return new Response<string> { Status = Status.Error.ToString(), Message = ex.Message, HttpStatus = HttpStatusCode.InternalServerError };
            }
        }
        
        [Authorize]
        [Route("~/api/ShareMovie")]
        [HttpPost]
        public async Task<Response<string>> SendEmail(Guid MovieId, string Email)
        {
            try
            {
                if (await _movieService.GetMovieById(MovieId) != null)
                {
                    var response = await _movieService.SendEmail(MovieId, Email);
                    return new Response<string> { Status = Status.Success.ToString(), Message = "Message Sent Successfully", HttpStatus = response.StatusCode };
                }
                return new Response<string> { Status = Status.NotFound.ToString(), Message = "Movie Not Found", HttpStatus = HttpStatusCode.NotFound };

            }
            catch (Exception ex)
            {
                return new Response<string> { Status = Status.Error.ToString(), Message = ex.Message, HttpStatus = HttpStatusCode.InternalServerError };
            }
        }
        


        [Route("~/api/GetAverageRating/{MovieId}")]
        [HttpGet]
        public async Task<Response<decimal>> GetAverage(Guid MovieId)
        {
            try
            {
                var averagerating = await _movieService.GetAverageRating(MovieId);
                return new Response<decimal> { Status = Status.Data.ToString(), Message = "Average rating" ,Data=averagerating};
            }
            catch (Exception ex)
            {
               return new Response<decimal> { Status = Status.Error.ToString(), Message = ex.Message, HttpStatus = HttpStatusCode.InternalServerError };
            }
        }
    }
}

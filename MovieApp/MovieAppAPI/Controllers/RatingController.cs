using Common.Enums;
using Common.ViewModels;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;
using System.Net;
using System.Security.Claims;

namespace MovieAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;
        private readonly IMovieService _movieService;

        public RatingController(IRatingService ratingService, IMovieService movieService)
        {
            _ratingService = ratingService;
            _movieService = movieService;
        }

        [Authorize]
        [HttpPost]
        [Route("~/api/Rating/CreateRating")]
        public async Task<Response<string>> PostRating(RateViewModel rate)
        {
            try
            {
                if (await _movieService.GetMovieById(rate.MovieId) != null)
                {
                    var RatedBy = User.FindFirstValue("UserId");
                    await _ratingService.AddRating(rate, RatedBy);
                    return new Response<string> { Status = Status.Success.ToString(), Message = "Rating Added Successfully", HttpStatus = HttpStatusCode.NoContent };
                }
                return new Response<string> { Status = Status.NotFound.ToString(), Message = "Movie Not Found", HttpStatus = HttpStatusCode.NotFound };

            }
            catch (Exception ex)
            {
                return new Response<string> { Status = Status.Error.ToString(), Message = ex.Message, HttpStatus = HttpStatusCode.InternalServerError };
            }
        }


        [Authorize]
        [HttpGet]
        [Route("~/api/Rating/HasRated")]
        public async Task<Response<bool>> GetHasRated(Guid MovieId)
        {
            try
            {
                if(await _movieService.GetMovieById(MovieId) !=null)
                {
                    var UserId = User.FindFirstValue("UserId");
                    if (await _ratingService.HasRated(MovieId, UserId))
                    {
                        return new Response<bool> { Status = Status.Success.ToString(), Message = "User Has Rated", HttpStatus = HttpStatusCode.OK, Data = true };
                    }
                    return new Response<bool> { Status = Status.Success.ToString(), Message = "User Has Not Rated", HttpStatus = HttpStatusCode.OK, Data = false };
                }
                else
                {
                    return new Response<bool> { Status = Status.NotFound.ToString(), Message = "Movie Not Found", HttpStatus = HttpStatusCode.NotFound };
                }

            }
            catch (Exception ex)
            {
                return new Response<bool> { Status = Status.Error.ToString(), Message = ex.Message, HttpStatus = HttpStatusCode.InternalServerError };
            }
        }
    }
}

using Common.Enums;
using Common.ViewModels;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;
using System.Security.Claims;

namespace MovieAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }
        
        
        
        [HttpPost]
        [Route("~/api/Genres/CreateGenere")]
        [Authorize(Roles = nameof(UserRole.Super_Admin) + "," + nameof(UserRole.Admin))]
        public async Task<Response<string>> PostGenre(string GenreName)
        {
            try
            {
                var UserId = User.FindFirstValue("UserId");
                await _genreService.AddGenre(GenreName,UserId);
                return new Response<string> { Status = Status.Success.ToString(), Message = "Gerne Added Successfully" };
            }
            catch(Exception ex)
            {
                return new Response<string> { Status = Status.Error.ToString(), Message = ex.Message, HttpStatus = HttpStatusCode.InternalServerError };
            }
        }

        
        
        [Authorize(Roles = nameof(UserRole.Super_Admin) + "," + nameof(UserRole.Admin))]
        [Route("~/api/DeleteGenre/{GenreId}")]
        [HttpDelete]
        public async Task<Response<string>> DeleteGenre(Guid GenreId)
        {
            try
            {
                if (await _genreService.GetGenreById(GenreId) != null)
                {
                    await _genreService.DeleteGenre(GenreId);
                    return new Response<string> { Status = Status.Success.ToString(), Message = "Gerne Deleted Successfully", HttpStatus = HttpStatusCode.NoContent };
                }
                return new Response<string> { Status = Status.Warning.ToString(), Message = "Gerne NotFound", HttpStatus = HttpStatusCode.NotFound };

            }
            catch (Exception ex)
            {
                return new Response<string> { Status = Status.Error.ToString(), Message = ex.Message, HttpStatus = HttpStatusCode.InternalServerError };
            }
        }



        //[Authorize(Roles = nameof(UserRole.Super_Admin) + "," + nameof(UserRole.Admin))]
        [Route("~/api/GetGenre/{GenreId}")]
        [HttpGet]
        public async Task<Response<GenreViewModel>> GetGenreById(Guid GenreId)
        {
            try
            {
                var genre = await _genreService.GetGenreById(GenreId);
                if(genre != null)
                    return new Response<GenreViewModel> { Status = Status.Success.ToString(), Message = "Found Genre by Id", HttpStatus = HttpStatusCode.Found, Data=genre };

                else
                    return new Response<GenreViewModel> { Status = Status.Warning.ToString(), Message = "Cannot Found Genre by Id", HttpStatus = HttpStatusCode.NotFound};

            }
            catch (Exception ex)
            {
                return new Response<GenreViewModel> { Status = Status.Error.ToString(), Message = ex.Message, HttpStatus = HttpStatusCode.InternalServerError };
            }
        }



        //[Authorize(Roles = nameof(UserRole.Super_Admin) + "," + nameof(UserRole.Admin))]
        [Route("~/api/GetGenre")]
        [HttpGet]
        public async Task<Response<IEnumerable<GenreViewModel>>> GetGenres()
        {
            try
            {
                var genres = await _genreService.GetAllGenre();
                if(genres != null && genres.Count()>=1 )
                    return new Response<IEnumerable<GenreViewModel>> { Status = Status.Success.ToString(), Message = "Found Genres", HttpStatus = HttpStatusCode.Found, Data = genres };

                else
                    return new Response<IEnumerable<GenreViewModel>> { Status = Status.Warning.ToString(), Message = "Genres Not Found", HttpStatus = HttpStatusCode.NotFound };

            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<GenreViewModel>> { Status = Status.Error.ToString(), Message = ex.Message, HttpStatus = HttpStatusCode.InternalServerError };
            }
        }



        [Authorize(Roles = nameof(UserRole.Super_Admin) + "," + nameof(UserRole.Admin))]
        [Route("~/api/UpdateGenre/{GenreId}")]
        [HttpPut]
        public async Task<Response<string>> UpdateGenre(Guid GenreId, string GenreName)
        {
            try
            {
                await _genreService.UpdateGenre(GenreId,GenreName);
                return new Response<string> { Status = Status.Success.ToString(), Message = "Gerne Updated Successfully", HttpStatus = HttpStatusCode.NoContent };
            }
            catch (Exception ex)
            {
                return new Response<string> { Status = Status.Error.ToString(), Message = ex.Message, HttpStatus = HttpStatusCode.InternalServerError };
            }
        }
    }
}

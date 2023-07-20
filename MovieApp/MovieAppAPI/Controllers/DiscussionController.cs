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
    public class DiscussionController : ControllerBase
    {
        private readonly IDiscussionService _discussionService;
        

        public DiscussionController(IDiscussionService discussionService)
        {
            _discussionService = discussionService;
            
        }


        [Authorize]
        [HttpPost]
        [Route("~/api/Discussion/CreateDiscussion")]
        public async Task<Response<string>> PostDiscussion(AddDiscussionViewModel discussion)
        {
            try
            {
                var UserId = User.FindFirstValue("UserId");
                await _discussionService.AddDiscussion(discussion, UserId);
                return new Response<string> { Status = Status.Success.ToString(), Message = "Discussion Added Successfully", HttpStatus = HttpStatusCode.NoContent };
            }
            catch (Exception ex)
            {
                return new Response<string> { Status = Status.Error.ToString(), Message = ex.Message, HttpStatus = HttpStatusCode.InternalServerError };
            }
        }



        [Authorize]
        [HttpDelete]
        [Route("~/api/Discussion/DeleteDiscussion/{DiscussionId}")]
        public async Task<Response<string>> DeleteDiscussion(Guid DiscussionId)
        {
            try
            {
                if (await _discussionService.GetDiscussionById(DiscussionId) != null)
                {
                    await _discussionService.DeleteDiscussionById(DiscussionId);
                    return new Response<string> { Status = Status.Success.ToString(), Message = "Discussion Deleted Successfully", HttpStatus = HttpStatusCode.NoContent };
                }
                return new Response<string> { Status = Status.NotFound.ToString(), Message = "Discussion Not Found", HttpStatus = HttpStatusCode.NotFound };
            }
            catch (Exception ex)
            {
                return new Response<string> { Status = Status.Error.ToString(), Message = ex.Message, HttpStatus = HttpStatusCode.InternalServerError };
            }
        }
        
        
        
        [HttpGet]
        [Route("~/api/Discussion/GetDiscussions/{MovieId}")]
        public async Task<Response<DiscussionsViewModel>> GetDiscussionsByMovie(Guid MovieId, int page=1)
        {
            try
            {
                var discussions = await _discussionService.GetDiscussionsByMovie(MovieId, page);
                if (discussions.Discussions != null && discussions.Discussions.Count()>0)
                {
                    return new Response<DiscussionsViewModel> { Status = Status.Data.ToString(), Message = "Discussions Found", HttpStatus = HttpStatusCode.OK, Data = discussions };
                }
                return new Response<DiscussionsViewModel> { Status = Status.NotFound.ToString(), Message = "No Discussions Found", HttpStatus = HttpStatusCode.NotFound };
            }
            catch (Exception ex)
            {
                return new Response<DiscussionsViewModel> { Status = Status.Error.ToString(), Message = ex.Message, HttpStatus = HttpStatusCode.InternalServerError };
            }
        }
    }
}

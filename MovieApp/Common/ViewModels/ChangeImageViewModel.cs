using Microsoft.AspNetCore.Http;

namespace Common.ViewModels
{
    public class ChangeImageViewModel
    {
        public Guid MovieId { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}

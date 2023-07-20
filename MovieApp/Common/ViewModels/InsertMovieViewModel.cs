using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;

namespace Common.ViewModels
{
    public class InsertMovieViewModel
    {
        [DisplayName("Movie Title")]
        [Required(ErrorMessage = "The Movie Title field is required.")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "The Movie Description field is required.")]
        [DisplayName("Movie Description")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "The Movie Duration field is required.")]
        [DisplayName("Movie Duration")]
        public string? MovieDuration { get; set; }
        [Required(ErrorMessage = "The Movie Genre field is required.")]
        [DisplayName("Movie Genre")]
        public Guid GenreId { get; set; }
        [Required(ErrorMessage = "The Release Date field is required.")]

        [DisplayName("Release Data")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        [DisplayName("Movie Image")]
        public IFormFile? ImageFile { get; set; }
    }
}

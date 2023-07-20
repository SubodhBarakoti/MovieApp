using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class Movie
    {
        [Key]
        public Guid Id { get; set; }

        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? MovieDuration { get; set; }
        [ForeignKey("Genre")]
        public Guid GenreId { get; set; }
        public Genres? Genre { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime AddedDate { get; set; }
        [ForeignKey("User")]
        public string? added_by { get; set; }
        public Users? User { get; set; }
        //can i do this??
        [DefaultValue(0)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal AverageRating { get; set; }
        public string? ImagePath { get; set; }
    }
}

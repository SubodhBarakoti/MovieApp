using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class Genres
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [DisplayName("Genre Name")]
        public string? Name { get; set; }

        [ForeignKey("User")]
        public string? Added_By { get; set; }
        public Users? User { get; set; }
        public DateTime Created_Date { get; set; }
        public ICollection<Movie>? Movies { get; set; }
    }
}

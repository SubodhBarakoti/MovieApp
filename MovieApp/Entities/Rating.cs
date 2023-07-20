using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class Rating
    {
        [Key]
        public Guid Id { get; set; }
        public int rating { get; set; }
        [ForeignKey("user")]
        public string? RatedBy { get; set; }
        public Users? user { get; set; }
        [ForeignKey("movie")]
        public Guid MovieId { get; set; }
        public Movie? movie { get; set; }
        public DateTime RatedDate { get; set; }
    }
}
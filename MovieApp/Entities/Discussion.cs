using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class Discussion
    {
        [Key]
        public Guid Id { get; set; }
        public string? DiscussionText { get; set; }
        [ForeignKey("User")]
        public string? UserId { get; set; }
        public Users? User { get; set; }
        [ForeignKey("Movie")]
        public Guid MovieId { get; set; }
        public Movie? Movie { get; set; }
        public DateTime? Created { get; set; }
    }
}

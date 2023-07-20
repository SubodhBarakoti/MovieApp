using DataAccessLayer.Repositories.Interface;

namespace DataAccessLayer.Repositories
{
    public class AllRepositories : IAllRepositories
    {

        public AllRepositories(
            IDiscussionRepository discussionRepository,
            IMovieRepository movieRepository,
            IGenreRepository genreRepository,
            IRatingRepository ratingRepository
            )
        {
            Movie = movieRepository;
            Discussion= discussionRepository;
            Genre = genreRepository;
            Rating = ratingRepository;
        }
        public IMovieRepository Movie { get; set; }
        public IRatingRepository Rating { get; set; }
        public IDiscussionRepository Discussion { get; set; }
        public IGenreRepository Genre { get; set; }
    }
}

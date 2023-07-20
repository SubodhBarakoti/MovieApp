using DataAccessLayer.Repositories.Interface;

namespace DataAccessLayer.Repositories
{
    public interface IAllRepositories
    {
        IMovieRepository Movie { get; set; }
        IRatingRepository Rating { get; set; }
        IDiscussionRepository Discussion { get; set; }
        IGenreRepository Genre { get; set; }
    }
}
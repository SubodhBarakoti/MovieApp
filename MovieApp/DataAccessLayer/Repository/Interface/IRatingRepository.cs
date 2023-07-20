using Entities;

namespace DataAccessLayer.Repositories.Interface
{
    public interface IRatingRepository
    {
        Task AddRating(Rating rating);
        Task<bool> HasRated(Guid MovieId, string UserId);
        Task<int> RatingCountByMovie(Guid MovieId);
        Task UpdateRating(Rating rating);
    }
}
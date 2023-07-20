using Common.ViewModels;

namespace Services.Interface
{
    public interface IRatingService
    {
        Task AddRating(RateViewModel rate, string RatedBy);
        Task<bool> HasRated(Guid MovieId, string UserId);
        Task<int> RatingCountByMovie(Guid MovieId);
    }
}
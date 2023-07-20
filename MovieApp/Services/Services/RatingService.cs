using Common.ViewModels;
using DataAccessLayer.Repositories;
using Entities;
using Services.Interface;

namespace Services.Services
{
    public class RatingService : IRatingService
    {
        private readonly IAllRepositories _repository;
        private readonly IMovieService _movieService;

        public RatingService(IAllRepositories repository, IMovieService movieService)
        {
            _repository = repository;
            _movieService = movieService;
        }
        public async Task AddRating(RateViewModel rate, string RatedBy)
        {
            Rating rating = new Rating()
            {
                Id = Guid.NewGuid(),
                rating = rate.rating,
                MovieId = rate.MovieId,
                RatedBy = RatedBy,
                RatedDate = DateTime.Now
            };
            int count = await RatingCountByMovie(rate.MovieId);
            await _repository.Rating.AddRating(rating);
            await _movieService.CalculateAverageRating(count, rate.rating, rate.MovieId);
        }
        public async Task<int> RatingCountByMovie(Guid MovieId)
        {
            return await _repository.Rating.RatingCountByMovie(MovieId);
        }
        public async Task<bool> HasRated(Guid MovieId, string? UserId)
        {
            return await _repository.Rating.HasRated(MovieId, UserId);
        }
    }
}

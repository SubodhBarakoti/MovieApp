using Microsoft.Extensions.DependencyInjection;
using Services.Interface;
using Services.Services;
using Microsoft.Extensions.Configuration;

namespace Services.Dependecy
{
    public static class ServiceDependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IPagerService, PagerService>();
            services.AddTransient<IMediaUploadService, MediaUploadService>();
            services.AddTransient<IMovieService, MovieService>();
            services.AddTransient<IDiscussionService, DiscussionService>();
            services.AddTransient<IRatingService, RatingService>();
            services.AddTransient<IGenreService, GenreService>();
            return services;
        }
    }
}

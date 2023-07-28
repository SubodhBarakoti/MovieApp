using Microsoft.Extensions.DependencyInjection;
using Services.Interface;
using Services.Services;
using Microsoft.Extensions.Configuration;
using Hangfire;

namespace Services.Dependecy
{
    public static class ServiceDependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddHangfire(config => config
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection"))
            );
            services.AddHangfireServer();

            services.AddTransient<IBackGroundServices, BackGroundServices>();
            services.AddTransient<IPagerService, PagerService>();
            services.AddTransient<IMediaUploadService, MediaUploadService>();
            services.AddTransient<IEmailServices, EmailServices>();
            services.AddTransient<IMovieService, MovieService>();
            services.AddTransient<IDiscussionService, DiscussionService>();
            services.AddTransient<IRatingService, RatingService>();
            services.AddTransient<IGenreService, GenreService>();
            return services;
        }
    }
}

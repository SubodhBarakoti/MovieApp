using DataAccessLayer.Persistance;
using DataAccessLayer.Persistance.Seed;
using DataAccessLayer.Repositories;
using DataAccessLayer.Repositories.Interface;
using DataAccessLayer.Repositories.Repository;
using DataAccessLayer.Repositories.SPRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccessLayer.Dependency
{
    public static class DataLayerDependencyInjection
    {
        public static IServiceCollection AddDataLinkLayerServices(this IServiceCollection services, IConfiguration configuration)
        {
            var ConnectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(ConnectionString, a => { a.MigrationsAssembly("DataAccessLayer"); }));

            services.AddScoped<IDbInitializer, DbInitializer>();


            

            services.AddTransient<IAllRepositories, AllRepositories>();

            if (configuration.GetValue<bool>("UseStoredProcedure"))
            {
                services.AddScoped<IMovieRepository, MovieSPRepository>();
                services.AddScoped<IGenreRepository, GenreSPRepository>();
                services.AddScoped<IDiscussionRepository, DiscussionSPRepository>();
                services.AddScoped<IRatingRepository, RatingSPRepository>();
            }
            else
            {
                services.AddScoped<IMovieRepository, MovieRepository>();
                services.AddScoped<IGenreRepository, GenreRepository>();
                services.AddScoped<IDiscussionRepository, DiscussionRepository>();
                services.AddScoped<IRatingRepository, RatingRepository>();
            }
            return services;

        }
    }
}

using Common.Enums;
using DataAccessLayer.Persistance;
using DataAccessLayer.Persistance.Dapper;
using DataAccessLayer.Persistance.Seed;
using DataAccessLayer.Repositories;
using DataAccessLayer.Repositories.Interface;
using DataAccessLayer.Repositories.Repository;
using DataAccessLayer.Repositories.SPRepository;
using DataAccessLayer.Repository.DapperRepository;
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


            services.AddScoped<DapperContext>();

            services.AddScoped<IDbInitializer, DbInitializer>();

            services.AddTransient<IAllRepositories, AllRepositories>();

            if (configuration["UseDataAccessTechnology"]==DataAccessTechnology.Ado_Net.ToString())
            {
                services.AddTransient<IMovieRepository, MovieSPRepository>();
                services.AddTransient<IGenreRepository, GenreSPRepository>();
                services.AddTransient<IDiscussionRepository, DiscussionSPRepository>();
                services.AddTransient<IRatingRepository, RatingSPRepository>();
            }
            else if(configuration["UseDataAccessTechnology"] == DataAccessTechnology.EF_Core.ToString())
            {
                services.AddTransient<IMovieRepository, MovieRepository>();
                services.AddTransient<IGenreRepository, GenreRepository>();
                services.AddTransient<IDiscussionRepository, DiscussionRepository>();
                services.AddTransient<IRatingRepository, RatingRepository>();
            }
            else if(configuration["UseDataAccessTechnology"] == DataAccessTechnology.Dapper.ToString())
            {
                services.AddTransient<IMovieRepository, DapperMovieRepository>();
                services.AddTransient<IGenreRepository, DapperGenreRepository>();
                services.AddTransient<IDiscussionRepository, DapperDiscussionRepository>();
                services.AddTransient<IRatingRepository, DapperRatingRepository>();
            }

            return services;
        }
    }
}

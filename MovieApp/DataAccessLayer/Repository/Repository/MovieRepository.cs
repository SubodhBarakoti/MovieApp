using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Persistance;
using DataAccessLayer.Repositories.Interface;
using Common.ViewModels;
using Entities;
using Common.Enums;

namespace DataAccessLayer.Repositories.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly ApplicationDbContext _context;

        public MovieRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task InsertMovie(Movie movie)
        {
            await _context.tbl_Movies.AddAsync(movie);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<MovieViewModel>> GetAllMovies
            (
            int skip_count,
            int page_size,
            Guid? GenreId,
            MovieSortBy movieSortBy
            )
        {
            IEnumerable<MovieViewModel> movies;
            if (GenreId == null)
            {
                switch (movieSortBy)
                {
                    case MovieSortBy.Name:
                        movies = await GetMoviesByName(skip_count, page_size);
                        break;
                    case MovieSortBy.AverageRating:
                        movies = await GetMoviesByRating(skip_count, page_size);
                        break;
                    case MovieSortBy.ReleaseDate:
                        movies = await GetMoviesByReleaseDate(skip_count, page_size);
                        break;
                    default:
                        movies = await GetMoviesByName(skip_count, page_size);
                        break;
                }
            }
            else
            {
                switch (movieSortBy)
                {
                    case MovieSortBy.Name:
                        movies = await GetMoviesByNameAndGenre(skip_count, page_size, GenreId);
                        break;
                    case MovieSortBy.AverageRating:
                        movies = await GetMoviesByRatingAndGenre(skip_count, page_size, GenreId);
                        break;
                    case MovieSortBy.ReleaseDate:
                        movies = await GetMoviesByReleaseDateAndGenre(skip_count, page_size, GenreId);
                        break;
                    default:
                        movies = await GetMoviesByNameAndGenre(skip_count, page_size, GenreId);
                        break;
                }
            }
            return movies;
        }
        public async Task<IEnumerable<MovieViewModel>> GetMoviesByName(int skipcount, int pagesize)
        {
            return await _context.tbl_Movies.Select(m => new MovieViewModel
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                GenreName = m.Genre.Name,
                MovieDuration=m.MovieDuration,
                ReleaseDate = m.ReleaseDate,
                ImagePath = m.ImagePath,
                AverageRating = m.AverageRating
            }).OrderBy(m => m.Name).Skip(skipcount).Take(pagesize).ToListAsync();
        }
        public async Task<IEnumerable<MovieViewModel>> GetMoviesByNameAndGenre(int skipcount, int pagesize, Guid? id)
        {
            return await _context.tbl_Movies
                .Where(m => m.GenreId == id)
                .Select(m => new MovieViewModel
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description,
                    GenreName = m.Genre.Name,
                    ReleaseDate = m.ReleaseDate,
                    ImagePath = m.ImagePath,
                    AverageRating = m.AverageRating
                })
                .OrderBy(m => m.Name).Skip(skipcount).Take(pagesize).ToListAsync();
        }
        public async Task<IEnumerable<MovieViewModel>> GetMoviesByRating(int skipcount, int pagesize)
        {
            return await _context.tbl_Movies.Select(m => new MovieViewModel
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                GenreName = m.Genre.Name,
                ReleaseDate = m.ReleaseDate,
                ImagePath = m.ImagePath,
                AverageRating = m.AverageRating
            }).OrderByDescending(m => m.AverageRating).Skip(skipcount).Take(pagesize).ToListAsync();
        }
        public async Task<IEnumerable<MovieViewModel>> GetMoviesByRatingAndGenre(int skipcount, int pagesize, Guid? id)
        {
            return await _context.tbl_Movies
                .Where(m => m.GenreId == id)
                .Select(m => new MovieViewModel
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description,
                    GenreName = m.Genre.Name,
                    ReleaseDate = m.ReleaseDate,
                    ImagePath = m.ImagePath,
                    AverageRating = m.AverageRating
                })
                .OrderByDescending(m => m.AverageRating).Skip(skipcount).Take(pagesize).ToListAsync();
        }
        public async Task<IEnumerable<MovieViewModel>> GetMoviesByReleaseDate(int skipcount, int pagesize)
        {
            return await _context.tbl_Movies.Select(m => new MovieViewModel
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                GenreName = m.Genre.Name,
                ReleaseDate = m.ReleaseDate,
                ImagePath = m.ImagePath,
                AverageRating = m.AverageRating
            }).OrderBy(m => m.ReleaseDate).Skip(skipcount).Take(pagesize).ToListAsync();
        }
        public async Task<IEnumerable<MovieViewModel>> GetMoviesByReleaseDateAndGenre(int skipcount, int pagesize, Guid? id)
        {
            return await _context.tbl_Movies
                .Where(m => m.GenreId == id)
                .Select(m => new MovieViewModel
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description,
                    GenreName = m.Genre.Name,
                    ReleaseDate = m.ReleaseDate,
                    ImagePath = m.ImagePath,
                    AverageRating = m.AverageRating
                })
                .OrderBy(m => m.ReleaseDate).Skip(skipcount).Take(pagesize).ToListAsync();
        }
        public async Task<MovieViewModel> GetViewMovieById(Guid id)
        {
             return await _context.tbl_Movies.Select(m => new MovieViewModel
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description,
                    GenreName = m.Genre.Name,
                    MovieDuration = m.MovieDuration,
                    ReleaseDate = m.ReleaseDate,
                    ImagePath = m.ImagePath,
                    AverageRating = m.AverageRating
                }).Where(m => m.Id == id).FirstOrDefaultAsync() ;
            
        }
        // how to remove the green line here
        public async Task<Movie> GetMovieById(Guid id)
        {
            return await _context.tbl_Movies.FindAsync(id);
        }
        public async Task DeleteMovie(Guid id)
        {
            var movie = await GetMovieById(id);
            _context.tbl_Movies.Remove(movie);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMovie(Movie movie)
        {
            _context.Entry(movie).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task<int> MovieCountByGenre(Guid? GenreId)
        {
            if (GenreId == null)
            {
                return await _context.tbl_Movies.CountAsync();
            }
            else
            {
                return await _context.tbl_Movies.Where(m => m.GenreId == GenreId).CountAsync();
            }
        }
        public async Task<decimal> GetAverageRating(Guid id)
        {
            return await _context.tbl_Movies.Where(m => m.Id == id).Select(m => m.AverageRating).FirstOrDefaultAsync();
        }

        public async Task UpdateImage(Guid MovieId, string ImagePath)
        {
            var movie = await GetMovieById(MovieId);
            movie.ImagePath= ImagePath;
            await _context.SaveChangesAsync();
        }
    }
}
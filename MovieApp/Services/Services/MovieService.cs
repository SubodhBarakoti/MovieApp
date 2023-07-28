using Common.Enums;
using Common.ViewModels;
using DataAccessLayer.Repositories;
using Entities;
using Microsoft.Extensions.Configuration;
using Services.Interface;

namespace Services.Services
{
    public class MovieService : IMovieService
    {
        private readonly IAllRepositories _repository;
        private readonly IMediaUploadService _mediaUploadService;
        private readonly IPagerService _pager;
        private readonly IConfiguration _configuration;
        private readonly string ProjectLocation;
        private readonly IEmailServices _emailServices;

        public MovieService
            (
            IAllRepositories repository, 
            IMediaUploadService mediaUploadService, 
            IPagerService pager, 
            IConfiguration configuration, 
            IEmailServices emailServices
            )
        {
            _repository = repository;
            _mediaUploadService = mediaUploadService;
            _pager = pager;
            _configuration = configuration;
            ProjectLocation = _configuration.GetValue<string>("ProjectLocation") ?? throw new KeyNotFoundException();
            _emailServices = emailServices;
        }
        public async Task InsertMovie(InsertMovieViewModel imovie, string UserId)
        {
            string ImageFolder = _configuration.GetValue<string>("ImageLocation")?? throw new ArgumentNullException("ImageFolderNotFound");
            string ImagePath = await _mediaUploadService.AddImage(imovie.ImageFile, ProjectLocation, ImageFolder);

            Movie movie = new()
            {
                Id = Guid.NewGuid(),
                Name = imovie.Name,
                Description = imovie.Description,
                MovieDuration = imovie.MovieDuration,
                GenreId = imovie.GenreId,
                ReleaseDate = imovie.ReleaseDate,
                added_by = UserId,
                AddedDate = DateTime.Now,
                ImagePath = ImagePath
            };
            await _repository.Movie.InsertMovie(movie);
        }
        public async Task UpdateMovie(Guid id, UpdateMovieViewModel umovie)
        {
            var movie = await GetMovieById(id);
            if (movie != null)
            {
                movie.Name = umovie.Name;
                movie.Description = umovie.Description;
                movie.MovieDuration = umovie.MovieDuration;
                movie.GenreId = umovie.GenreId;
                movie.ReleaseDate = umovie.ReleaseDate;
                await _repository.Movie.UpdateMovie(movie);
            }
        }
        public async Task<DisplayMoviesViewModel> GetAllMovies(Guid? GenreId, MovieSortBy sortBy, int pageno)
        {
            int size = 3;
            int movie_count;
            PagerViewModel pager = new PagerViewModel();
            DisplayMoviesViewModel displayMovies= new DisplayMoviesViewModel();
            if (GenreId == null)
            {
                movie_count = await _repository.Movie.MovieCountByGenre(GenreId);
            }
            else
            {
                movie_count = await _repository.Movie.MovieCountByGenre(GenreId);
            }
            if(movie_count > 0)
            {
                _pager.PagerInitialize(movie_count, pageno, size);
                int skip_count = (_pager.CurrentPage - 1) * _pager.PageSize;
                pager.CurrentPage = _pager.CurrentPage;
                pager.TotalPages = _pager.TotalPages;
                pager.StartPage = _pager.StartPage;
                pager.EndPage = _pager.EndPage;
                IEnumerable<MovieViewModel> movies = await _repository.Movie.GetAllMovies(skip_count, size, GenreId, sortBy);
                
                {
                    displayMovies.Movies = movies;
                    displayMovies.pager = pager;
                    displayMovies.GenreId = GenreId;
                    displayMovies.SortBy = sortBy;
                }

                return displayMovies;
            }
            return displayMovies;
        }
        public async Task<Movie> GetMovieById(Guid id)
        {
            var movie = await _repository.Movie.GetMovieById(id);
            //var movie =await GetViewMovieById(id);
            return movie;
        }
        public async Task<MovieViewModel> GetViewMovieById(Guid id)
        {
            return await _repository.Movie.GetViewMovieById(id);
        }
        public async Task ChangeImage(ChangeImageViewModel uimage)
        {
            var movie = await GetViewMovieById(uimage.MovieId);
            if (movie != null && uimage.ImageFile != null)
            {
                string ImageFolder = _configuration.GetValue<string>("ImageLocation")?? throw new Exception();
                string ImagePath = await _mediaUploadService.ChangeImage(uimage.ImageFile, ProjectLocation, ImageFolder, movie.ImagePath??throw new Exception());
                movie.ImagePath = ImagePath;
                await _repository.Movie.UpdateImage(movie.Id,ImagePath);
            }
        }
        public async Task<UpdateMovieViewModel> GetEditItemById(Guid id)
        {
            Movie movie = await GetMovieById(id);
            return new UpdateMovieViewModel
            {
                Id = movie.Id,
                Name = movie.Name,
                Description = movie.Description,
                MovieDuration = movie.MovieDuration,
                GenreId = movie.GenreId,
                ReleaseDate = movie.ReleaseDate
            };
        }
        public async Task DeleteMovie(Guid id)
        {
            var movie = await GetMovieById(id);
            if (movie != null)
            {
                await _repository.Movie.DeleteMovie(id);
            }
        }
        public async Task<decimal> GetAverageRating(Guid id)
        {
            return await _repository.Movie.GetAverageRating(id);
        }
        public async Task CalculateAverageRating(int count, int rate, Guid MovieId)
        {
            var movie = await GetMovieById(MovieId);

            if (movie != null)
            {
                var newAverage = (count * movie.AverageRating + rate) / (count + 1);
                movie.AverageRating = newAverage;
                await _repository.Movie.UpdateMovie(movie);
            }
        }

        public async Task<dynamic> SendEmail(Guid MovieId, string Email)
        {
            var movie = await GetViewMovieById(MovieId);
            //if (movie != null)
            {
                string filepath = Path.Combine(ProjectLocation, "SendEmailFormat.html");

                string imagepath = Path.Combine(ProjectLocation, movie.ImagePath);

                byte[] imageData = File.ReadAllBytes(imagepath);
                string imageBase64 = Convert.ToBase64String(imageData);

                string imageExtension = Path.GetExtension(imagepath).TrimStart('.');
                string imageMimeType = $"image/{imageExtension}";
                string image = $"data:{imageMimeType};base64,{imageBase64}";

                var layout = await File.ReadAllTextAsync(filepath);

                layout = layout.Replace("##Title##","MovieApp");
                //layout = layout.Replace("##imagesLink##", image);
                layout = layout.Replace("##ImageName##",movie.Name);
                layout = layout.Replace("##movieTitle##",movie.Name);
                layout = layout.Replace("##movieDescription##",movie.Description);
                layout = layout.Replace("##MovieDuration##",movie.MovieDuration);
                layout = layout.Replace("##Genre##",movie.GenreName);
                layout = layout.Replace("##ReleaseDate##",movie.ReleaseDate.ToString());
                layout = layout.Replace("##AverageRating##",movie.AverageRating.ToString());
                EmailServiceViewModel emailServiceViewModel = new()
                {
                    Subject="A movie have been Recommended",
                    ReceiverEmail=Email,
                    UserName=Email,
                    Message="",
                    HtmlContent=layout
                };
                return await _emailServices.SendSMTPEmail(emailServiceViewModel);
            }
        }
    }
}

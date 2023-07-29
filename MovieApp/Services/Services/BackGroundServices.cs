using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.ViewModels;
using DataAccessLayer.Persistance;
using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.Interface;

namespace Services.Services
{
    public class BackGroundServices : IBackGroundServices
    {
        private readonly IMovieService _movieService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Users> _userManager;
        private readonly IEmailServices _emailService;

        public BackGroundServices(IMovieService movieService, ApplicationDbContext context, UserManager<Users> userManager, IEmailServices emailService)
        {
            _movieService = movieService;
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task SendNotificationMovieRelease()
        {
            DateTime tomorrow = DateTime.Today.AddDays(1);

            // Query the database to get movies with release date equal to tomorrow
            var moviesReleasingTomorrow = await _context.tbl_Movies
                .Where(m => m.ReleaseDate.Date == tomorrow.Date)
                .Select(m => m.Name)
                .ToListAsync();
            if(moviesReleasingTomorrow != null)
            {
                string movies = "";
                foreach (var movie in moviesReleasingTomorrow)
                {
                    movies = movie + ", ";
                }
                BackGroundTaskEmail email = new BackGroundTaskEmail();
                email.Subject = "Movie Release Date Tomorrow";
                email.ReceiverEmail = await Allmails();
                email.Message = movies + "are releasing tomorrow. So Visit the nearest hall to get to the action";

                await _emailService.BackGroundTaskEmail(email);
            }
            Console.WriteLine($"Running the process at {DateTime.Now.ToString("yyyy-mm-dd HH-mm-ss")}");
        }
        public async Task<IEnumerable<string>> Allmails()
        {
            var allusers = await _userManager.Users.ToListAsync();
            var allEmails = allusers.Select(u => u.Email).ToList();
            return allEmails;
        }
    }
}

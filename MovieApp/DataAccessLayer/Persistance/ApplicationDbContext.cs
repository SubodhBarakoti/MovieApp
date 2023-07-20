using Microsoft.EntityFrameworkCore;
using Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DataAccessLayer.Persistance
{
    public class ApplicationDbContext : IdentityDbContext<Users>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Movie> tbl_Movies { get; set; }
        public DbSet<Rating> tbl_Ratings { get; set; }
        public DbSet<Discussion> tbl_Discussions { get; set; }
        public DbSet<Genres> tbl_Genres { get; set; }
    }
}
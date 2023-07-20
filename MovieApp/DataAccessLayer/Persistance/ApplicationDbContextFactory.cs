//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Design;

//namespace DataAccessLayer.Persistance
//{
//    // this is to scaffold the entities
//    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
//    {
//        public ApplicationDbContext CreateDbContext(string[] args)
//        {
//            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
//            optionsBuilder.UseSqlServer("Server=SUBODH;Database=MovieApp;Trusted_Connection=True;MultipleActiveResultSets=true;Trust Server Certificate=True;");
//            return new ApplicationDbContext(optionsBuilder.Options);
//        }
//    }
//}

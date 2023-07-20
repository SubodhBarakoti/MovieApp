namespace DataAccessLayer.Persistance.Seed
{
    public interface IDbInitializer
    {
        Task Initialize();
    }
}
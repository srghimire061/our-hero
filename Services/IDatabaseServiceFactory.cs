using OurHeroWebAPI.Entities;

namespace OurHeroWebAPI.Services
{
    public interface IDatabaseServiceFactory
    {
        IDatabaseService GetDatabaseService(string dbType, string connectionString, OurHeroDbContext dbContext);
    }

}

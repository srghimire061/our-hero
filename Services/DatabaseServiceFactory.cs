using OurHeroWebAPI.Entities;

namespace OurHeroWebAPI.Services
{
    public class DatabaseServiceFactory: IDatabaseServiceFactory
    {
        public IDatabaseService GetDatabaseService(string dbType, string connectionString, OurHeroDbContext dbContext = null)
        {
            if (string.IsNullOrEmpty(dbType))
                throw new ArgumentNullException(nameof(dbType), "Database type cannot be null or empty");

            switch (dbType.ToLower())
            {
                case "sql":
                    return new OurHeroServiceADO(connectionString); // ADO.NET service using connection string
                case "ef":
                    return new OurHeroServiceDB(dbContext); // EF service using DbContext
                default:
                    throw new ArgumentException($"Unsupported database type: {dbType}");
            }
        }
    }
}

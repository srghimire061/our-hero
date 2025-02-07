using Microsoft.AspNetCore.Mvc;
using OurHeroWebAPI.Entities;
using OurHeroWebAPI.Services;

namespace OurHeroWebAPI.Controllers
{
    [Route("api/factory")]
    [ApiController]
    public class FactoryImplementationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly OurHeroDbContext _dbContext;
        private readonly IDatabaseServiceFactory _dbFactory;

        public FactoryImplementationController(IConfiguration configuration, OurHeroDbContext dbContext, IDatabaseServiceFactory dbFactory)
        {
            _configuration = configuration;
            _dbContext = dbContext;
            _dbFactory = dbFactory;
        }

        private IDatabaseService GetDatabaseService(string dbType)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            return _dbFactory.GetDatabaseService(dbType, connectionString, _dbContext);
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string dbType, [FromQuery] bool? isActive = null)
        {
            var service = GetDatabaseService(dbType);
            var heros = await service.GetAllHeros(isActive);
            return Ok(heros);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, [FromQuery] string dbType)
        {
            var service = GetDatabaseService(dbType);
            var hero = await service.GetHerosByID(id);
            if (hero == null)
            {
                return NotFound();
            }
            return Ok(hero);
        }
    }
}

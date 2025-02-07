using OurHeroWebAPI.Models;

namespace OurHeroWebAPI.Services
{
    public interface IDatabaseService
    {
        Task<List<OurHero>> GetAllHeros(bool? isActive = null);
        Task<OurHero> GetHerosByID(int id);
        Task<OurHero> AddOurHero(AddUpdateOurHero heroObject);
        Task<OurHero> UpdateOurHero(int id, AddUpdateOurHero heroObject);
        Task<bool> DeleteHerosByID(int id);
    }
}

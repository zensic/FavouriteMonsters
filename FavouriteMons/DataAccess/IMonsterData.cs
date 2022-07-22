using FavouriteMons.Models;
using Refit;

namespace FavouriteMons.DataAccess
{
    public interface IMonsterData
    {
        [Get("/Monsters")]
        Task<List<Monsters>> GetMonsters();

        [Get("/Monsters/{id}")]
        Task<Monsters> GetMonsters(Guid id);

        [Post("/Guests")]
        Task CreateMonster([Body] Monsters monster);

        [Put("/Guest/{id}")]
        Task UpdateMonster(Guid id, [Body] Monsters monster);

        [Delete("/Guests/{id}")]
        Task DeleteMonster(Guid id);
    }
}

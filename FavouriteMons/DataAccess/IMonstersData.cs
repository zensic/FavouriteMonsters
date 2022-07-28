using FavouriteMons.Models;
using Refit;

namespace FavouriteMons.DataAccess
{
    public interface IMonstersData
    {
        [Get("/Monsters")]
        Task<List<Monsters>> GetMonsters();

        [Post("/Monsters")]
        Task CreateMonster([Body] Monsters monster);

        [Get("/Monsters/{id}")]
        Task<Monsters> GetMonsters(Guid id);

        [Put("/Monsters/{id}")]
        Task UpdateMonster(Guid id, [Body] Monsters monster);

        [Delete("/Monsters/{id}")]
        Task DeleteMonster(Guid id);
    }
}

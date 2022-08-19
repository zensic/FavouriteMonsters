using FavouriteMons.Models;
using Refit;

namespace FavouriteMons.DataAccess
{
  [Headers("Authorization: 52cc004d-b5c8-448f-be6b-6b9bd532fe20")]
  public interface IMonstersData
  {
    [Get("/Monsters")]
    Task<IEnumerable<object>> GetMonsters();

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

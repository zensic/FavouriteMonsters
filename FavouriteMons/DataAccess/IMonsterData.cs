using FavouriteMons.Models;
using Refit;

namespace FavouriteMons.DataAccess
{
    public interface IMonsterData
    {
        [Get("/Guests")]
        Task<List<Monster>> GetGuests();
    }
}

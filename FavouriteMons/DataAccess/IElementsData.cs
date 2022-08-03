using FavouriteMons.Models;
using Refit;


namespace FavouriteMons.DataAccess
{
    public interface IElementsData
    {
        [Get("/Elements")]
        Task<List<Elements>> GetElements();
    }
}

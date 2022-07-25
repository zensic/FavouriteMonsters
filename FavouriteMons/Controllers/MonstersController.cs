using FavouriteMons.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace FavouriteMons.Controllers
{
    public class MonstersController : Controller
    {
        private readonly IMonstersData _monstersData;

        public MonstersController(IMonstersData monstersData)
        {
            _monstersData = monstersData;
        }

        public async Task<IActionResult> Index()
        {
            var monsters = await _monstersData.GetMonsters();

            return View(monsters);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            return View(_monstersData.GetMonsters(id));
        }
    }
}

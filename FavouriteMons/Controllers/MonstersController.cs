using FavouriteMons.Areas.Identity.Data;
using FavouriteMons.DataAccess;
using FavouriteMons.Models;
using Microsoft.AspNetCore.Mvc;

namespace FavouriteMons.Controllers
{
    public class MonstersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMonstersData _monstersData;

        public MonstersController(ApplicationDbContext context, IMonstersData monstersData)
        {
            _context = context;
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

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Element,ImageUrl")] Monsters monsters, IFormFile[] images)
        {
            if (ModelState.IsValid)
            {
                monsters.Id = Guid.NewGuid();
                _context.Add(monsters);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(monsters);
        }
    }
}

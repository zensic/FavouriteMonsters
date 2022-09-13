using FavouriteMons.Areas.Identity.Data;
using FavouriteMons.DataAccess;
using FavouriteMons.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FavouriteMons.Controllers
{
  public class HomeController : Controller
  {
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;
    private readonly IMonstersData _monstersData;
    private readonly IElementsData _elementsData;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IMonstersData monstersData, IElementsData elementsData)
    {
      _logger = logger;
      _context = context;
      _monstersData = monstersData;
      _elementsData = elementsData;
    }

    public async Task<IActionResult> Index()
    {
      var monsters = await _monstersData.GetMonsters();

      ViewBag.monsters = monsters;

      return View();
    }

    public IActionResult Privacy()
    {
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
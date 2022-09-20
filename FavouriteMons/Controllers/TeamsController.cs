using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FavouriteMons.Areas.Identity.Data;
using FavouriteMons.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using FavouriteMons.DataAccess;
using System.Text.Json;
using Newtonsoft.Json;

namespace FavouriteMons.Controllers
{
  [Authorize]
  public class TeamsController : Controller
  {
    private readonly ApplicationDbContext _context;
    private readonly IMonstersData _monstersData;
    private readonly ITeamsData _teamsData;

    public TeamsController(ApplicationDbContext context, IMonstersData monstersData, ITeamsData teamsData)
    {
      _context = context;
      _monstersData = monstersData;
      _teamsData = teamsData;
    }

    // GET: Teams
    public async Task<IActionResult> Index()
    {
      ViewBag.teamDisplayList = await _teamsData.GetTeamsById(Guid.Parse(ClaimTypes.NameIdentifier));

      return View();
    }

    // GET: Teams/Create
    public async Task<IActionResult> Create()
    {
      ViewData["UserId"] = User.FindFirstValue(ClaimTypes.NameIdentifier);
      ViewBag.monsters = await _monstersData.GetMonsters();

      return View();
    }

    // GET: Teams/MonsterDetails
    public async Task<object> GetMonster(Guid id)
    {
      var monster = await _monstersData.GetDetailsMonster(id);
      string serialized = JsonConvert.SerializeObject(monster);

      return serialized;
    }

    // POST: Teams/Create
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TeamNew teamNew)
    {
      var result = await _teamsData.CreateTeam(teamNew);

      return result;
    }
  }
}

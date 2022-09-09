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

    public TeamsController(ApplicationDbContext context, IMonstersData monstersData)
    {
      _context = context;
      _monstersData = monstersData;
    }

    // GET: Teams
    public async Task<IActionResult> Index()
    {
      // Returns a list of team objects
      // Each team has date created
      // Each team has a list of monster objects

      // Grab all teams first 
      List<TeamDisplay> teamDisplayList = new();

      List<Teams> teamList = await (from teams in _context.Teams
                                    where teams.UserId == Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))
                                    select teams)
                              .AsNoTracking()
                              .ToListAsync();

      // Populate the monster teams with userId and datetime
      if (teamList.Count > 0)
      {
        foreach (Teams team in teamList)
        {
          teamDisplayList.Add(new TeamDisplay(team.Id, team.UserId, team.CreatedAt));
        }
      }
      else
      {
        ViewBag.teamDisplayList = teamDisplayList;
        return View();
      }

      // Populate the monster teams into the teamList
      for (int i = 0; i < teamDisplayList.Count; i++)
      {
        // Grab monster ids of monsters in the team
        List<TeamMonsters> teamMonstersList = await (from teamMonsters in _context.TeamMonsters
                                                     where teamMonsters.TeamId == teamDisplayList[i].Id
                                                     select teamMonsters)
                                                     .AsNoTracking()
                                                     .ToListAsync();

        // Grab monster data from monster API
        foreach (TeamMonsters teamMonsters in teamMonstersList)
        {
          teamDisplayList[i].Monsters.Add(await _monstersData.GetMonsters(teamMonsters.MonsterId));
        }
      }

      ViewBag.teamDisplayList = teamDisplayList;
      return View();
    }

    // GET: Teams/Details/Guid
    public async Task<IActionResult> Details(Guid? id)
    {
      if (id == null || _context.Teams == null)
      {
        return NotFound();
      }

      var teams = await _context.Teams
          .FirstOrDefaultAsync(m => m.Id == id);
      if (teams == null)
      {
        return NotFound();
      }

      return View(teams);
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
      // Generate unique id for teams
      Guid teamGuid = Guid.NewGuid();

      // Add each selected team monster to team monsters table
      foreach (Guid teamMonsterId in teamNew.MonsterIds)
      {
        _context.Add(new TeamMonsters(teamGuid, teamMonsterId));
        await _context.SaveChangesAsync();
      }

      // Add new team to teams table
      Teams teamTemp = new();
      teamTemp.Id = teamGuid;
      teamTemp.UserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

      _context.Add(teamTemp);
      await _context.SaveChangesAsync();

      return RedirectToAction("Index");
    }

    // GET: Teams/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
      if (id == null || _context.Teams == null)
      {
        return NotFound();
      }

      var teams = await _context.Teams.FindAsync(id);
      if (teams == null)
      {
        return NotFound();
      }
      return View(teams);
    }

    // POST: Teams/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("Id,UserId,CreatedAt,Monsters")] Teams teams)
    {
      if (id != teams.Id)
      {
        return NotFound();
      }

      if (ModelState.IsValid)
      {
        try
        {
          _context.Update(teams);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!TeamsExists(teams.Id))
          {
            return NotFound();
          }
          else
          {
            throw;
          }
        }
        return RedirectToAction(nameof(Index));
      }
      return View(teams);
    }

    // GET: Teams/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
      if (id == null || _context.Teams == null)
      {
        return NotFound();
      }

      var teams = await _context.Teams
          .FirstOrDefaultAsync(m => m.Id == id);
      if (teams == null)
      {
        return NotFound();
      }

      return View(teams);
    }

    // POST: Teams/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
      if (_context.Teams == null)
      {
        return Problem("Entity set 'ApplicationDbContext.Teams'  is null.");
      }
      var teams = await _context.Teams.FindAsync(id);
      if (teams != null)
      {
        _context.Teams.Remove(teams);
      }

      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }

    private bool TeamsExists(Guid id)
    {
      return (_context.Teams?.Any(e => e.Id == id)).GetValueOrDefault();
    }
  }
}

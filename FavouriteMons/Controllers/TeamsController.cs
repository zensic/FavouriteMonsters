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
      var teamList = await (from teams in _context.Teams
                            join teamMonsters in _context.TeamMonsters on teams.Id equals teamMonsters.TeamId
                            where teams.UserId == Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))
                            select new
                            {
                              TeamId = teams.Id,
                              DateCreated = teams.CreatedAt,

                            })
                            .AsNoTracking()
                            .ToListAsync();

      ViewBag.teamList = teamList;

      return View();
    }

    // GET: Teams/Details/5
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

    // POST: Teams/Create
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TeamNew teamNew)
    {
      // Generate unique id for teams
      Guid teamGuid = new Guid();

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

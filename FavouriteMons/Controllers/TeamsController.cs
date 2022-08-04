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
      return _context.Teams != null ?
                  View(await _context.Teams.ToListAsync()) :
                  Problem("Entity set 'ApplicationDbContext.Teams'  is null.");
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
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("UserId,CreatedAt,Monsters")] Teams teams)
    {
      if (ModelState.IsValid)
      {
        teams.Id = Guid.NewGuid();
        _context.Add(teams);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }
      return View(teams);
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

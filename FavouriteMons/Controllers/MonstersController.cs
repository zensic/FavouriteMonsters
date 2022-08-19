using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FavouriteMons.Areas.Identity.Data;
using FavouriteMons.DataAccess;
using FavouriteMons.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FavouriteMons.Controllers
{
  public class MonstersController : Controller
  {
    private readonly Cloudinary _cloudinary;
    private readonly ApplicationDbContext _context;
    private readonly IMonstersData _monstersData;
    private readonly IElementsData _elementsData;

    public MonstersController(Cloudinary cloudinary, ApplicationDbContext context, IMonstersData monstersData, IElementsData elementsData)
    {
      _cloudinary = cloudinary;
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

    public async Task<IActionResult> Details(Guid id)
    {
      return View(await _monstersData.GetMonsters(id));
    }

    public async Task<IActionResult> Create()
    {
      List<Elements> elements = await _elementsData.GetElements();

      List<SelectListItem> types = new List<SelectListItem>();

      for (int i = 0; i < elements.Count; i++)
      {
        types.Add(new SelectListItem(elements[i].Name, elements[i].Id.ToString()));
      }

      ViewBag.types = types;

      return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name, ElementId")] Monsters monster, IFormFile[] images)
    {
      // Handle image upload with Cloudinary
      if (images == null || images.Length == 0)
      {
        monster.ImageUrl = "placholder";
      }
      else
      {
        var result = await _cloudinary.UploadAsync(new ImageUploadParams
        {
          File = new FileDescription(images[0].FileName,
                images[0].OpenReadStream()),
        }).ConfigureAwait(false);

        monster.ImageUrl = result.Url.AbsoluteUri;
        //Bytes = (int)result.Bytes,
        //CreatedAt = DateTime.Now,
        //Format = result.Format,
        //Height = result.Height,
        //Path = result.Url.AbsolutePath,
        //PublicId = result.PublicId,
        //ResourceType = result.ResourceType,
        //SecureUrl = result.SecureUrl.AbsoluteUri,
        //Signature = result.Signature,
        //Type = result.JsonObj["type"]?.ToString(),
        //Url = result.Url.AbsoluteUri,
        //Width = result.Width
      }

      // Add monster to database
      await _monstersData.CreateMonster(monster);

      return RedirectToAction("Index");
    }
  }
}

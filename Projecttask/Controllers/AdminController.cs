using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Projecttask.Data;
using Projecttask.Models;
using Projecttask.Models.ViewModels;

namespace Projecttask.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult AddCategorie()
    {
        return View();
    }

    public IActionResult Edit()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Edit(int id, Tag editedtag)
    {
        if (ModelState.IsValid)
        {
            var tag = _context.Tag.Find(id);

            tag.Name = editedtag.Name;

            _context.Entry(tag).State = EntityState.Modified;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        return View(editedtag);
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        var categorie = _context.Tag.Find(id);
        if (categorie != null)
        {
            _context.Tag.Remove(categorie);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View();
    }

    [HttpPost]
    public IActionResult AddCategorie(AddCategoryViewModel model)
    {
        if (ModelState.IsValid)
        {
            var newCategory = new Tag
            {
                Name = model.TagName,
            };

            _context.Tag.Add(newCategory);
            _context.SaveChanges();

            return RedirectToAction("Categories");
        }
        return View(model);
    }


    public IActionResult Categories()
    {
        var categories = _context.Tag.ToList();
        return View(categories);
    }
}

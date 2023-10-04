using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projecttask.Data;
using Projecttask.Models;
using Projecttask.Models.ViewModels;

namespace Projecttask.Areas.Admin.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;
    public AdminController(UserManager<ApplicationUser> userManager, ApplicationDbContext context) 
    {
        _userManager = userManager;
        _context = context;
    }

	public async Task<IActionResult> Index()
	{
        var usersWithTags = new List<UserWithTagRoleViewModel>();
        var users = _userManager.Users.ToList();
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var tags = _context.UserTag.Where(ut => ut.UserId == user.Id).Select(ut => ut.Tag).ToList();
            var userWithTagViewModel = new UserWithTagRoleViewModel
            {
                User = user,
                Tags = tags,
                Roles = roles
            };

            usersWithTags.Add(userWithTagViewModel);
        }

        ViewBag.Users = usersWithTags;
        return View();
	}

    public IActionResult Tags()
    {
        List<Tag> allTags = _context.Tag.ToList();
        ViewBag.Tags = allTags;
        return View();
    }

    [HttpPost]
    public IActionResult tags(AddCategoryViewModel vm)
    {
        if (vm != null)
        {
            _context.Tag.Add(new Tag() { Name = vm.TagName});
            _context.SaveChanges();
        }
        return RedirectToAction("tags");
    }

    public IActionResult DeleteTag(int id)
    {
        var tag = _context.Tag.FirstOrDefault(tag => tag.Id == id);
        if (tag != null)
        {
            _context.Tag.Remove(tag);
            _context.SaveChanges();
        }
        return RedirectToAction("tags");
    }

    public async Task<IActionResult> LockUser(string Id)
    {
        var user = await _userManager.FindByIdAsync(Id);
        if (user != null)
        {
            user.LockoutEnabled = false;
            await _userManager.UpdateAsync(user);
        }
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> UnlockUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user != null)
        {
            user.LockoutEnabled = true;
            await _userManager.UpdateAsync(user);
        }
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        var orders = _context.Orders.Where(order => order.WorkerId == id || order.EmployerId == id);
        _context.Orders.RemoveRange(orders);
        _context.SaveChanges();
        var result = await _userManager.DeleteAsync(user);

        if (result.Succeeded)
        {
            return RedirectToAction("Index");
        }
        else
        {
            ModelState.AddModelError("", "Error deleting the user.");
            return RedirectToAction("Index");
        }
    }
}

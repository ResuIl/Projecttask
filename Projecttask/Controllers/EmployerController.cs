using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Projecttask.Data;
using Projecttask.Models;
using Projecttask.Models.ViewModels;

namespace Projecttask.Controllers;

[Authorize(Roles = "Employer")]
public class EmployerController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;

    public EmployerController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var workers = await _userManager.GetUsersInRoleAsync("Worker");
		var usersWithTags = new List<UserWithTagViewModel>();

		foreach (var worker in workers)
		{
			// Retrieve tags associated with the user (you may need to implement this method)
			var tags = _context.UserTag.Where(ut => ut.UserId == worker.Id).Select(ut => ut.Tag).ToList();

			// Create a UserWithTagViewModel and add it to the list
			var userWithTagViewModel = new UserWithTagViewModel
			{
				User = worker,
				Tags = tags
			};

			usersWithTags.Add(userWithTagViewModel);
		}

		return View(usersWithTags);
    }

	public async Task<IActionResult> Profile(string? userid)
	{
		if (string.IsNullOrEmpty(userid))
		{
			// Handle the case where userId is not provided or is invalid
			return BadRequest("Invalid user ID");
		}

		var user = await _userManager.FindByIdAsync(userid);

		if (user == null)
		{
			return NotFound("User not found");
		}
			
        return View(user);
	}
}

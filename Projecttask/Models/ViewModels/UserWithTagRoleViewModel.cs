using Microsoft.AspNetCore.Identity;

namespace Projecttask.Models.ViewModels;

public class UserWithTagRoleViewModel
{
    public ApplicationUser User { get; set; }
    public List<Tag> Tags { get; set; }
    public IList<string> Roles { get; set; }
}

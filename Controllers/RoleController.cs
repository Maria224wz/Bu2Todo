namespace BU2Todo;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

[ApiController]
[Route("todo/roles")]
public class RoleController : ControllerBase
{
    UserManager<User> userManager;
    RoleManager<IdentityRole> roleManager;

    public RoleController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        this.userManager = userManager;
        this.roleManager = roleManager;
    }

    [HttpPost("create")]    // Borde ha en Authorize, men blir strul när andra tar ner projektet då användarna sparas lokalt och kan därmed inte tilldela en ny admin-roll.
    public async Task<string> CreateRole([FromQuery] string name)
    {
        await roleManager.CreateAsync(new IdentityRole(name));
        return "Created role " + name;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddUserToRole([FromQuery] string role, [FromQuery] string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound("User was not found!");
        }

        await userManager.AddToRoleAsync(user, role);
        return Ok("Added Role" + role + " to user " + user.UserName);
    }
}
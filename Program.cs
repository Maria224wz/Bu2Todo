using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BU2Todo;

public class Program
{
    public static void Main(string[] args)

    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<ApplicationContext>(options =>
        { // tjänst för app context med anslutningssträng
            options.UseNpgsql(
                "Host=localhost;Database=todoapp;Username=postgres;Password=password"
            );
        });

        builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);

        builder.Services.AddAuthorization(options =>
        {

            options.AddPolicy("GetAllTodos", policy =>
            {
                policy.RequireAuthenticatedUser().RequireRole("admin");
            });

            options.AddPolicy("Admin", policy =>
            {
                policy.RequireAuthenticatedUser().RequireRole("admin");
            });

            options.AddPolicy("UserAddTodo", policy =>
            {
                policy.RequireAuthenticatedUser();

            });

            options.AddPolicy("UserDeleteTodo", policy =>
            {
                policy.RequireAuthenticatedUser();
            });

            options.AddPolicy("GetUserTodos", policy =>
            {
                policy.RequireAuthenticatedUser();
            });

            options.AddPolicy("UserUpdateTodos", policy =>
            {
                policy.RequireAuthenticatedUser();

            });
        });

        builder.Services.AddControllers();
        builder.Services.AddTransient<IClaimsTransformation, UserClaimsTransformation>();

        SetupSecurity(builder);
        builder.Services.AddScoped<TodoService, TodoService>();

        var app = builder.Build();
        app.MapIdentityApi<User>();
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }

    public static void SetupSecurity(WebApplicationBuilder builder)
    {
        builder.Services
            .AddIdentityCore<User>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationContext>()
            .AddApiEndpoints();
    }
}

public class UserClaimsTransformation : IClaimsTransformation
{
    readonly UserManager<User> userManager;

    public UserClaimsTransformation(UserManager<User> userManager)
    {
        this.userManager = userManager;
    }

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        ClaimsIdentity claims = new ClaimsIdentity();

        var id = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (id != null)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                var userRoles = await userManager.GetRolesAsync(user);
                foreach (var userRole in userRoles)
                {
                    claims.AddClaim(new Claim(ClaimTypes.Role, userRole));
                }
            }
        }

        principal.AddIdentity(claims);
        return await Task.FromResult(principal);
    }
}

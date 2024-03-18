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

        builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme); // token autentifiering läggs till

        builder.Services.AddAuthorization(options =>
        { // autentiserings funktionalitet och konfiguerar behörighet

            options.AddPolicy("GetAllTodos", policy =>
            {
                policy.RequireAuthenticatedUser();
            }); // behörighetspolicy för att hämta alla todos och användaren måste vara autentiserad

            options.AddPolicy("Admin", policy =>
            {
                policy.RequireAuthenticatedUser();
            });

            options.AddPolicy("UserAddTodo", policy =>
            {
                policy.RequireAuthenticatedUser();
                
            });
        });

        builder.Services.AddControllers(); // controllers för att hantera http androp
          //builder.Services.AddTransient<IClaimsTransformation();

        SetupSecurity(builder); // konfigurera säkerhet 
        builder.Services.AddScoped<TodoService, TodoService>();

        var app = builder.Build();

        app.MapIdentityApi<User>();

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization(); // autentifiering och authentisering för behöringhetskontroller, authentication ska ligga först
       

        app.MapControllers();

        app.Run();
    }

    public static void SetupSecurity(WebApplicationBuilder builder)
    {
        builder.Services
            .AddIdentityCore<User>() // identity tjänster med user som användarklass
            .AddRoles<IdentityRole>() // roll tjänster
            .AddEntityFrameworkStores<ApplicationContext>() // lagra användarinfo
            .AddApiEndpoints(); // api endpoints för identity funktioner
    }

 }

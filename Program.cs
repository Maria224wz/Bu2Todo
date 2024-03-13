using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BU2Todo;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAuthorization(options =>
        {

            options.AddPolicy("GetAllTodos", policy =>
            {
                policy.RequireAuthenticatedUser();
            });

            options.AddPolicy("Admin", policy =>
            {
                policy.RequireAuthenticatedUser();
            });
        });

        builder.Services.AddControllers();
        SetupSecurity(builder);
        builder.Services.AddScoped<TodoService, TodoService>();

        builder.Services.AddDbContext<ApplicationContext>(options =>
        {
            options.UseNpgsql(
                "Host=localhost;Database=todoapp;Username=postgres;Password=password"
            );
        });

        builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);

        builder.Services.AddIdentityCore<User>()
        .AddEntityFrameworkStores<ApplicationContext>()
        .AddApiEndpoints();

        var app = builder.Build();

        app.MapIdentityApi<User>();

        app.UseHttpsRedirection();

        app.UseAuthorization();
        app.UseAuthentication();

        app.MapControllers();

        app.Run();
    }

    public static void SetupSecurity(WebApplicationBuilder builder)
    {
        builder
            .Services.AddIdentityCore<User>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationContext>()
            .AddApiEndpoints();
    }


}

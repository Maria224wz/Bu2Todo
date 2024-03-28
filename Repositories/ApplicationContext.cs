using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BU2Todo;

public class ApplicationContext : IdentityDbContext<User>
{
    public DbSet<Todo> Todos { get; set; }
    public DbSet<TodoList> TodoItems { get; set; }


    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }


}
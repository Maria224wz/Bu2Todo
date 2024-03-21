using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BU2Todo;

public class ApplicationContext : IdentityDbContext<User> // app context klass som ärver från identitydbcontext med user för användar autentisering med uppsättningar av todos och todolist
{
    public DbSet<Todo> Todos { get; set; }
    public DbSet<TodoList> TodoItems { get; set; }
    

    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }


}
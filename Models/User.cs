using Microsoft.AspNetCore.Identity;

namespace BU2Todo;

public class User : IdentityUser
{
    public List<Todo>? TodoItems { get; set; } = new List<Todo>();
}
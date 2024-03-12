using Microsoft.AspNetCore.Identity;

namespace BU2Todo;

public class User : IdentityUser // Ärv ifrån identityuser istället så behöver vi inte deklarera lösenord, användarnamn och ID
{
    public List<Todo>? TodoItems { get; set; } = new List<Todo>();
}
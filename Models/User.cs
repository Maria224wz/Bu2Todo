using Microsoft.AspNetCore.Identity;

namespace BU2Todo;

public class User : IdentityUser // User som ärver ifrån identityuser istället så behöver vi inte deklarera lösenord, användarnamn och ID
{
    public List<Todo>? TodoItems { get; set; } = new List<Todo>(); // listan av todoobjekt
}
using System.Security.Claims;
namespace BU2Todo;

public class TodoService
{
    ApplicationContext context; //variabel för databas anslutning

    public TodoService(ApplicationContext context)
    { // konstruktor som tar emot appcontext som berorende
        this.context = context; // tilldelar app context till variabeln context
    }
 
    public Todo CreateTodos(string title, string description, string duedate) // skapa ny todo
    {

        if (string.IsNullOrWhiteSpace(title)) // felhantering
        {
            throw new Exception("Title cannot be null or whitespace!");
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new Exception("Description cannot be null or whitespace!");
        }

    //      // Hämta användaren från databasen baserat på användar-ID- där Users innehåller info automatiskt om användaren
    //    User? user = context.Users.Find(User);

    // if (user == null)
    // {
    //     throw new ArgumentException("User not found!");
    // }     

        var todo = new Todo
        { // ny todo skapas med angivna uppgifter
            Title = title,
            Description = description,
            DueDate = duedate,
            
            
        };
        context.Todos.Add(todo); // lägg till nya i databasen
        //user.Todos.Add(todo); // Spara listan så att user kommer med i todo
        // context.user.Add(todo);
        context.SaveChanges(); // spara ändringar till databasen

        return todo; // returnerar den nya todon
    }

    public void DeleteAllTodos()
    {
        var todosToRemove = context.Todos.ToList(); // hämta alla todos från db
        context.Todos.RemoveRange(todosToRemove); // ta bort dem
        context.SaveChanges(); // spara ändringar
    }
}


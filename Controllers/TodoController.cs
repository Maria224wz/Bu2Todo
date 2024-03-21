using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BU2Todo;

[ApiController] // klassen är api controller
[Route("todo")]
public class TodoController : ControllerBase // ärver från controller base
{
    private readonly ApplicationContext context; // deklaration av en privat variabel för databas anslutning
    private readonly TodoService todoService; // en variabel för todo hantering i todservice

    public TodoController(ApplicationContext context, TodoService todoService) // konstruktor för appcontext och todoservice
    {
        this.context = context; // tilldela appcontext till context
        this.todoService = todoService; // tilldela todoservice till todoservice variabeln
    }

    [HttpPost]
    [Authorize("UserAddTodo")]
    public IActionResult CreateTodo(
        [FromQuery] string title,
        [FromQuery] string description,
        [FromQuery] string duedate
    ) //
    {
        // Hämta användarens id från ClaimsPrincipal - genom claimtypes så läggs info om token värdet in som gör att man kan sedan hämta användarobjektet genom att komma åt id't.
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Hämta användarobjektet från databasen, letar efter anv id som kommer från claimtypes och tilldelas då att bli user, annars user not found
        User? user = context.Users.FirstOrDefault(u => u.Id == userId);
         


        if (user == null)
        {
            // Om användaren inte hittas, returnera en lämplig felrespons
            return NotFound("User not found");
        }

        Todo todo = todoService.CreateTodos(title, description, duedate);
        todo.User = user;

        // Sätt CreatedDate till aktuellt datum och tid

        todo.CreatedDate = DateTime.UtcNow;
        // Konverterar CreatedDate till UTC och formatera det till år mån dag (tid kommer med ändå)
        string formattedDate = todo.CreatedDate.ToString("yyyy-MM-dd");

        // kollar om todotems är null först innan en todo läggs till i listan
        if (user.TodoItems == null)
        {
            user.TodoItems = new List<Todo>();
        }
        user.TodoItems.Add(todo);

        // context.TodoItems.Add;
        context.SaveChanges();

        // tilldela en todo till användaren

        //  todo.Title = title;
        //  todo.Description = description;
        //  todo.DueDate = duedate;




        return Ok(new TodoDto(todo)); // ok med nya todo som en dto// Try catch felhantering?
    }

// // hämta todos baserat på användarens email kopplat till id. Och Lagt till policy i mainmetoden för get user todos

[HttpGet("users")]
[Authorize("GetUserTodos")]
public IActionResult GetUserTodos()
{
    // Hämta alla användare från databasen
    List<User> users = context.Users.Include(u => u.TodoItems).ToList();

    // Skapa en lista av DTO:er för användar-ID, e-postadresser och todos
    List<UserTodosDto> userTodosDtos = users.Select(user => new UserTodosDto
    {
        UserEmail = new UserEmailDto
        {
            UserId = user.Id,
            Email = user.Email
        },
        Todos = user.TodoItems.Select(todo => new TodoDto(todo)).ToList()
    }).ToList();

    return Ok(userTodosDtos); // Returnera listan med användar-ID, e-postadresser och todos
}


    [HttpGet] // Hämtar alla oavsett user, lägg till authorization för admin
    [Authorize("GetAllTodos")]
    //[AllowAnonymous]
    public List<TodoDto> GetAllTodos() // hämta alla todos
    {
        return context.Todos.ToList().Select(todo => new TodoDto(todo)).ToList(); // hämta alla todos från db och konvertera dem som en dto
    }

    [HttpDelete]
    [Authorize("Admin")]
    //[AllowAnonymous]
    public IActionResult RemoveAllTodos() // Lägg till authorization för admin
    {
        todoService.DeleteAllTodos(); // anropar todoservice och deletealltodos
        return Ok("Removed all todos");
    }

    public class TodoDto
    { // egenskaper för id, title, description etc...
        public Guid Id { get; set; }

        public string Title { get; set; } = "";

        public string Description { get; set; } = "";

        public bool Completed = false;

        public DateTime CreatedDate { get; set; }

        public string DueDate { get; set; } = "";

        public TodoDto(Todo todo)
        { // konstruktor som tar en todo som en parameter och skapar en dto från den och tilldelar titel, descriptioin, id etc..
            this.Title = todo.Title;
            this.Description = todo.Description;
            this.Id = todo.Id;
            this.CreatedDate = todo.CreatedDate; // fixa utskriften till endast år / månad / dag / tid
            this.DueDate = todo.DueDate;
        }
    }

    public class UserEmailDto
{
    public string? UserId { get; set; }
    public string? Email { get; set; }
}


    public class UserTodosDto
    {
        public UserEmailDto? UserEmail { get; set; }
        public List<TodoDto>? Todos { get; set; }
    }
}

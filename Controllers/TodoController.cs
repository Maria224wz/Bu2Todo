using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    public IActionResult CreateTodo([FromQuery] string title, [FromQuery] string description, [FromQuery] string duedate) // 
    {
          // Hämta användarens id från ClaimsPrincipal
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Skapa todo för den aktuella användaren
             Todo todo = todoService.CreateTodos(title, description, duedate, userId);

        // Todo todo = todoService.CreateTodos(title, description, duedate);
        // todo.Title = title;
        // todo.Description = description;
        // todo.DueDate = duedate;
        

        return Ok(new TodoDto(todo));   // ok med nya todo som en dto// Try catch felhantering?
    }

    [HttpGet]       // Hämtar alla oavsett user, lägg till authorization för admin
    [Authorize("GetAllTodos")]
    //[AllowAnonymous]
    public List<TodoDto> GetAllTodos() // hämta alla todos
    {
        return context.Todos.ToList().Select(todo => new TodoDto(todo)).ToList(); // hämta alla todos från db och konvertera dem som en dto
    }

    [HttpDelete]
    [Authorize("Admin")]
    //[AllowAnonymous]
    public IActionResult RemoveAllTodos()       // Lägg till authorization för admin
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
            this.CreatedDate = DateTime.Now;        // fixa utskriften till endast år / månad / dag / tid
            this.DueDate = todo.DueDate;
            
        }

    }
}
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BU2Todo;

[ApiController]
[Route("todo")]
public class TodoController : ControllerBase
{
    private readonly ApplicationContext context;
    private readonly TodoService todoService;

    public TodoController(ApplicationContext context, TodoService todoService)
    {
        this.context = context;
        this.todoService = todoService;
    }

    [HttpPost]
    public IActionResult CreateTodo([FromQuery] string title, [FromQuery] string description, [FromQuery] string duedate)
    {
        Todo todo = todoService.CreateTodos(title, description, duedate);
        todo.Title = title;
        todo.Description = description;
        todo.DueDate = duedate;

        return Ok(new TodoDto(todo));   // Try catch felhantering?
    }

    [HttpGet]       // Hämtar alla oavsett user, lägg till authorization för admin
    [Authorize("GetAllTodos")]
    public List<TodoDto> GetAllTodos()
    {
        return context.Todos.ToList().Select(todo => new TodoDto(todo)).ToList();
    }

    [HttpDelete]
    [Authorize("Admin")]
    public IActionResult RemoveAllTodos()       // Lägg till authorization för admin
    {
        todoService.DeleteAllTodos();
        return Ok("Removed all todos");
    }

    public class TodoDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = "";

        public string Description { get; set; } = "";

        public bool Completed = false;

        public DateTime CreatedDate { get; set; }

        public string DueDate { get; set; } = "";

        public TodoDto(Todo todo)
        {
            this.Title = todo.Title;
            this.Description = todo.Description;
            this.Id = todo.Id;
            this.CreatedDate = DateTime.Now;        // fixa utskriften till endast år / månad / dag / tid
            this.DueDate = todo.DueDate;
        }

    }
}
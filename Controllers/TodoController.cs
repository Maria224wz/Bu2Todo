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
    public IActionResult CreateTodo([FromQuery] string title, [FromQuery] string description)
    {
        var todo = todoService.CreateTodos(title, description);
        todo.Title = title;
        todo.Description = description;
        return Ok();
    }

    public class CreateTodoDto
    {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";

    }
    public class TodoDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = "";

        public string Description { get; set; } = "";

        public bool Completed = false;

        public DateTime CreatedDate { get; set; }

        public DateTime DueDate { get; set; }

        public TodoDto(Todo todo)
        {
            this.Title = todo.Title;
            this.Description = todo.Description;
        }

    }
}
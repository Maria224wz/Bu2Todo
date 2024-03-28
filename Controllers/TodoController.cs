using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    [Authorize("UserAddTodo")]
    public IActionResult CreateTodo(
        [FromQuery] string title,
        [FromQuery] string description,
        [FromQuery] string duedate
    )

    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        User? user = context.Users.FirstOrDefault(u => u.Id == userId);

        if (user == null)
        {
            return NotFound("User not found");
        }

        Todo todo = todoService.CreateTodos(title, description, duedate);
        todo.User = user;

        todo.CreatedDate = DateTime.UtcNow;
        string formattedDate = todo.CreatedDate.ToString("yyyy-MM-dd");

        if (user.TodoItems == null)
        {
            user.TodoItems = new List<Todo>();
        }

        user.TodoItems.Add(todo);
        context.SaveChanges();
        return Ok(new TodoDto(todo));
    }

    [HttpGet("user")]
    [Authorize("GetUserTodos")]
    public IActionResult GetUserTodos()
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        string? userEmail = User.FindFirstValue(ClaimTypes.Email);

        User? user = context.Users
            .Include(u => u.TodoItems)
            .FirstOrDefault(u => u.Id == userId && u.Email == userEmail);

        if (user == null)
        {
            return NotFound("User not found");
        }

        List<TodoDto> userTodos = user.TodoItems.Select(todo => new TodoDto(todo)).ToList();

        var response = new
        {
            UserId = user.Id,
            Email = user.Email,
            Todos = userTodos
        };

        return Ok(response);
    }

    [HttpDelete("UserDelete")]
    [Authorize("UserDeleteTodo")]
    public IActionResult UserDeleteTodo([FromQuery] string title)
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        string? userEmail = User.FindFirstValue(ClaimTypes.Email);

        // Hämta bara den inloggade användarens todos
        Todo? todo = context.Todos
            .Include(t => t.User)
            .FirstOrDefault(t => t.Title == title && t.User.Id == userId && t.User.Email == userEmail);

        if (todo == null)
        {
            return NotFound("Todo not found");
        }

        context.Remove(todo);
        context.SaveChanges();

        return Ok("The todo was sucessfully removed");
    }

    [HttpPut("title")]
    [Authorize("UserUpdateTodos")]
    public IActionResult UpdateTodos(string title, [FromBody] TodoUpdateDto todoUpdateDto)
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        Todo? todo = context
            .Todos.Include(t => t.User)
            .FirstOrDefault(t => t.Title == title && t.User.Id == userId);

        if (todo == null)
        {
            return NotFound("Todo not found");
        }

        todo.CreatedDate = DateTime.UtcNow;

        if (!string.IsNullOrEmpty(todoUpdateDto.Description))
        {
            todo.Description = todoUpdateDto.Description;
        }

        if (!string.IsNullOrEmpty(todoUpdateDto.DueDate))
        {
            todo.DueDate = todoUpdateDto.DueDate;
        }

        context.SaveChanges();
        return Ok(new TodoDto(todo));
    }

    [HttpGet]
    [Authorize("GetAllTodos")]
    public List<TodoDto> GetAllTodos()
    {
        return context.Todos.ToList().Select(todo => new TodoDto(todo)).ToList();
    }

    [HttpDelete]
    [Authorize("Admin")]
    public IActionResult RemoveAllTodos()
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
            this.CreatedDate = todo.CreatedDate;
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
    public class TodoUpdateDto
    {
        public string? Description { get; set; }
        public string? DueDate { get; set; }
    }
}


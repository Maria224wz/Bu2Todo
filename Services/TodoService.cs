namespace BU2Todo;

public class TodoService
{
    ApplicationContext context;

    public TodoService(ApplicationContext context)
    {
        this.context = context;
    }

    public Todo CreateTodos(string title, string description)
    {

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new Exception("Title cannot be null or whitespace!");
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new Exception("Description cannot be null or whitespace!");
        }

        var todo = new Todo
        {
            Title = title,
            Description = description
        };

        context.Todos.Add(todo);
        context.SaveChanges();

        return todo;
    }

}
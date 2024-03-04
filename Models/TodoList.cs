namespace BU2Todo;

public class TodoList
{
    public Guid Id { get; set; }
    public List<Todo>? TodoItems { get; set; }

    public TodoList() { }

}
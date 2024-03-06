namespace BU2Todo;

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

}
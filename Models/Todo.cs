namespace BU2Todo;

public class Todo
{
    public Guid Id { get; set; }

    public string Title { get; set; } = "";

    public string Description { get; set; } = "";

    public bool Completed = false;

    public DateTime CreatedDate { get; set; }

    public string DueDate { get; set; } = "";     // Representera som sträng istället?

    public User? User {get; set;} = null;

}
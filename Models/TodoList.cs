namespace BU2Todo;

public class TodoList
// lista som representerar listan av todo objekt
{
    public Guid Id { get; set; } // id för todon i listan
    public List<Todo>? TodoItems { get; set; }  // lista av todoobjekt

    public TodoList() { }

}
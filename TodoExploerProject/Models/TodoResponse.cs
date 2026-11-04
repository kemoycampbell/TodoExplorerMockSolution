namespace TodoExploerProject.Models;

public class TodoResponse
{
    public List<TodoModel> Todos { get; set; }
    public int Total { get; set; }
    public int Skip { get; set; }
    public int Limit { get; set; }
}
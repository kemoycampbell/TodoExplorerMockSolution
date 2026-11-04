namespace TodoExploerProject.Models;

public class TodoFilter: ITodoFilter
{
    public int? UserId { get; set; }
    public bool? Completed { get; set; }
    public string? SearchTerm { get; set; }
}
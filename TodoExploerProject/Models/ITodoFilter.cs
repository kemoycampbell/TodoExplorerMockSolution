namespace TodoExploerProject.Models;

public interface ITodoFilter
{
    public int? UserId { get; set; }
    public bool? Completed { get; set; }
    public string? SearchTerm { get; set; }
}
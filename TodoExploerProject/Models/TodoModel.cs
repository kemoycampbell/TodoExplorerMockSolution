using CommunityToolkit.Mvvm.ComponentModel;

namespace TodoExploerProject.Models;

public partial class TodoModel: ObservableObject
{
    public String Todo { get; set; }
    public bool Completed { get; set; }
    public int Id { get; set; }
    public int UserId { get; set; }
    
    [ObservableProperty]
    private bool isFavorite;
}
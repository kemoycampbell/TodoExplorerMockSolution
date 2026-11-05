using CommunityToolkit.Mvvm.Input;
using TodoExploerProject.Models;

namespace TodoExploerProject.ViewModels;

public interface ITodoDetailViewModel
{
    public IAsyncRelayCommand GoBackToMainPageCommand { get; }
    public TodoModel SelectedTodo { get; set; }
    
}
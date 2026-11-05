using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using TodoExploerProject.Models;

namespace TodoExploerProject.ViewModels;

public interface IMainPageViewModel
{
    public ObservableCollection<TodoModel> Todos { get; set; }
    public Task LoadTodos();
    public IAsyncRelayCommand<TodoModel> ToggleFavoriteCommand { get; }
    public IAsyncRelayCommand<TodoModel> ShowDetailsCommand { get; }
    public IAsyncRelayCommand ReloadTodosCommand { get; }
    public IAsyncRelayCommand FavoritesNavPageCommand { get; }
    
    public string SearchText { get; set; }
    public string CompletionFilter { get; set; }
    
    public int SearchUserId { get; set; }
    
    public IAsyncRelayCommand FilterTodosCommand { get; }
    
    public bool IsLoading { get; set; }
}
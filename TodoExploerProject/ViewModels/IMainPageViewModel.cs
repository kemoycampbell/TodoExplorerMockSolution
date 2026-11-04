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
    public IAsyncRelayCommand ReloadTodosCommand { get; }
    public IAsyncRelayCommand FavoritesNavPageCommand { get; }
    
    public bool IsLoading { get; set; }
}
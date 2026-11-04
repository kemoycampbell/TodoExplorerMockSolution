using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TodoExploerProject.Models;
using TodoExploerProject.Services;

namespace TodoExploerProject.ViewModels;

public partial class MainPageViewModel:ObservableObject, IMainPageViewModel
{

    private readonly ITodoService todoService;

    [ObservableProperty] 
    private ObservableCollection<TodoModel> todos;
    
    [ObservableProperty]
    private bool isLoading;
    
    
    public MainPageViewModel(
        ITodoService todoService,
        ObservableCollection<TodoModel> todos
    )
    {
        this.todoService = todoService;
        this.todos = todos;
    }
    
    [RelayCommand]
    public async Task ToggleFavorite(TodoModel todo)
    {
        if (todo is null) return;
        
        //toggle between true and false
        todo.IsFavorite = !todo.IsFavorite;
    }

    [RelayCommand]
    public async Task FavoritesNavPage()
    {
        
    }

    [RelayCommand]
    public async Task ReloadTodos()
    {
        //clear the collection and reload
        Todos.Clear();
        await LoadTodos();

    }
    
    
    public async Task LoadTodos()
    {
        this.IsLoading = true;
        TodoResponse todoResponse = await todoService.GetTodos(new TodoFilter());
        Todos = new ObservableCollection<TodoModel>(todoResponse.Todos);
        this.IsLoading = false;
    }
}
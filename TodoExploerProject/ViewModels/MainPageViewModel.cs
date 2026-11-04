using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TodoExploerProject.Models;
using TodoExploerProject.Services;

namespace TodoExploerProject.ViewModels;

public partial class MainPageViewModel:ObservableObject, IMainPageViewModel
{

    private readonly ITodoService todoService;
    private IFavoriteStorage storage;
    private readonly INavigationService navigationService;

    [ObservableProperty] 
    private ObservableCollection<TodoModel> todos;
    
    [ObservableProperty]
    private bool isLoading;
    
    
    
    public MainPageViewModel(
        ITodoService todoService,
        ObservableCollection<TodoModel> todos,
        IFavoriteStorage storage,
        INavigationService navigationService
    )
    {
        this.todoService = todoService;
        this.todos = todos;
        this.storage = storage;
        this.navigationService = navigationService;
    }
    
    [RelayCommand]
    public async Task ToggleFavorite(TodoModel todo)
    {
        if (todo is null) return;
        
        //toggle between true and false
        todo.IsFavorite = !todo.IsFavorite;
        
        //update the favorite storage
        List<TodoModel> favorites = todos.Where(x => x.IsFavorite).ToList();
        storage.SaveFavorites(favorites);
    }

    [RelayCommand]
    public async Task FavoritesNavPage()
    {
        List<TodoModel> favoriteModels = await storage.GetFavorites();
        ObservableCollection<TodoModel> favorites = new ObservableCollection<TodoModel>(favoriteModels);
        await this.navigationService.GoToFavorites(favorites);
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
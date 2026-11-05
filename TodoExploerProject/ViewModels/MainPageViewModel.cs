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
    private readonly IFavoriteViewModel favoriteViewModel;
    private readonly ITodoDetailViewModel todoDetailViewModel;

    [ObservableProperty] 
    private ObservableCollection<TodoModel> todos;
    
    [ObservableProperty]
    private bool isLoading;
    
    [ObservableProperty]
    private string searchText;
    
    [ObservableProperty]
    private string completionFilter;

    [ObservableProperty] 
    private int searchUserId = -1;

    
    public MainPageViewModel(
        ITodoService todoService,
        ObservableCollection<TodoModel> todos,
        IFavoriteStorage storage,
        IFavoriteViewModel favoriteViewModel,
        INavigationService navigationService,
        ITodoDetailViewModel todoDetailViewModel
    )
    {
        this.todoService = todoService;
        this.todos = todos;
        this.storage = storage;
        this.favoriteViewModel = favoriteViewModel;
        this.navigationService = navigationService;
        this.todoDetailViewModel = todoDetailViewModel;
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
    public async Task ShowDetails(TodoModel todo)
    {
        if (todo is null) return;
        
        this.todoDetailViewModel.SelectedTodo = todo;
        await this.navigationService.GoToToDoDetail(this.todoDetailViewModel);
    }

    [RelayCommand]
    public async Task FavoritesNavPage()
    {
        List<TodoModel> favoriteModels = await storage.GetFavorites();
        ObservableCollection<TodoModel> favorites = new ObservableCollection<TodoModel>(favoriteModels);
        this.favoriteViewModel.Todos = favorites;
        await this.navigationService.GoToFavorites(this.favoriteViewModel);
    }

    [RelayCommand]
    public async Task ReloadTodos()
    {
        //clear the collection and reload
        Todos.Clear();
        await LoadTodos();

    }
    
    [RelayCommand]
    public async Task FilterTodos()
    {
       //setup a blank filter
        ITodoFilter filter = new TodoFilter();
        
        //check what the user has entered in the search text and completion filter and user id
        
        //if the user has entered a search term, we will filter by that
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            filter.SearchTerm = SearchText;
        }
        
        if(!string.IsNullOrWhiteSpace(CompletionFilter))
        {
            //we are only interested in incompleted or completed... all is the default search so we will not filter by that
            if(CompletionFilter.Equals("Completed", StringComparison.OrdinalIgnoreCase))
            {
                filter.Completed = true;
            }
            else if(CompletionFilter.Equals("Incompleted", StringComparison.OrdinalIgnoreCase))
            {
                filter.Completed = false;
            }
        }
        
        //if the user has entered a user id, we will filter by that
        if(SearchUserId > 0)
        {
            filter.UserId = SearchUserId;
        }
        
        //now we can apply the filter
        this.IsLoading = true;
        this.Todos.Clear();
        //get the filtered todos from the service
        TodoResponse todoResponse = await todoService.GetTodos(filter);
        
        //update thhe collection
        this.Todos = new ObservableCollection<TodoModel>(todoResponse.Todos);
        this.IsLoading = false;
        
    }


    public async Task LoadTodos()
    {
        this.IsLoading = true;
        TodoResponse todoResponse = await todoService.GetTodos(new TodoFilter());
        Todos = new ObservableCollection<TodoModel>(todoResponse.Todos);
        this.IsLoading = false;
    }
}
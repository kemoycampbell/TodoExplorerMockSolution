using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TodoExploerProject.Models;
using TodoExploerProject.Services;

namespace TodoExploerProject.ViewModels;

public partial class FavoriteViewModel: ObservableObject, IFavoriteViewModel
{
    private INavigationService navigationService;
    
    [ObservableProperty] 
    private ObservableCollection<TodoModel> todos;
    

    [ObservableProperty] 
    private bool isLoading;

    public FavoriteViewModel(
        INavigationService navigationService,
        ObservableCollection<TodoModel> todos
    )
    {
        this.navigationService = navigationService;
        this.todos = todos;
    }

    [RelayCommand]
    public async Task GoBackToMainPage()
    {
        await this.navigationService.PopAsync();
    }

}
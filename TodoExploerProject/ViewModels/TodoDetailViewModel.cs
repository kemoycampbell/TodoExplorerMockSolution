using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TodoExploerProject.Models;
using TodoExploerProject.Services;

namespace TodoExploerProject.ViewModels;

public partial class TodoDetailViewModel: ObservableObject, ITodoDetailViewModel
{
    private INavigationService navigationService;
    
    public TodoDetailViewModel(INavigationService navigationService)
    {
        this.navigationService = navigationService;
    }
    
    [ObservableProperty]
    private TodoModel selectedTodo;

    [RelayCommand]
    public async Task GoBackToMainPage()
    {
        await this.navigationService.PopAsync();
    }
}
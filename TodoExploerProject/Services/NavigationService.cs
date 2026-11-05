using System.Collections.ObjectModel;
using TodoExploerProject.Models;
using TodoExploerProject.ViewModels;
using TodoExploerProject.Views;

namespace TodoExploerProject.Services;

public class NavigationService: INavigationService
{
    private INavigation? navigation;
    
    public void setNavigation(INavigation navigation)
    {
        this.navigation = navigation;
    }

    public async Task PopAsync()
    {
        await this.navigation.PopAsync();
    }

    public async Task GoToToDoDetail(ITodoDetailViewModel viewModel)
    {
        await this.navigation.PushAsync(new TodoDetail(viewModel));
        
    }

    public async Task GoToFavorites(IFavoriteViewModel viewModel)
    {
        await this.navigation.PushAsync(new Favorites(viewModel));
    }
}
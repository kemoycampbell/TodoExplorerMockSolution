using System.Collections.ObjectModel;
using TodoExploerProject.Models;
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

    public async Task GoToToDoDetail(TodoModel todoModel)
    {
        await this.navigation.PushAsync(new TodoDetail(todoModel));
        
    }

    public async Task GoToFavorites(ObservableCollection<TodoModel> favorites)
    {
        await this.navigation.PushAsync(new Favorites(favorites));
    }
}
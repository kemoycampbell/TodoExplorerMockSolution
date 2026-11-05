using System.Collections.ObjectModel;
using TodoExploerProject.Models;

namespace TodoExploerProject.Services;

public interface INavigationService
{
    public void setNavigation(INavigation navigation);
    public Task PopAsync();
    public Task GoToToDoDetail(TodoModel todoModel);
    public Task GoToFavorites(ObservableCollection<TodoModel> favorites);
}
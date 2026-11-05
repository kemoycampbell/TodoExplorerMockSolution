using System.Collections.ObjectModel;
using TodoExploerProject.Models;
using TodoExploerProject.ViewModels;

namespace TodoExploerProject.Services;

public interface INavigationService
{
    public void setNavigation(INavigation navigation);
    public Task PopAsync();
    public Task GoToToDoDetail(ITodoDetailViewModel viewModel);
    public Task GoToFavorites(IFavoriteViewModel viewModel);
}
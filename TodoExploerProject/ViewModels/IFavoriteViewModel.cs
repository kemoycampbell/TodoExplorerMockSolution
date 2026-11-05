using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using TodoExploerProject.Models;

namespace TodoExploerProject.ViewModels;

public interface IFavoriteViewModel
{
    public ObservableCollection<TodoModel> Todos { get; set; }
    
    public IAsyncRelayCommand GoBackToMainPageCommand { get; }
    
    public bool IsLoading { get; set; }
}
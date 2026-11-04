using TodoExploerProject.Models;

namespace TodoExploerProject.Services;

public interface IFavoriteStorage
{
    public Task<List<TodoModel>> GetFavorites();
    public Task SaveFavorites(List<TodoModel> favorites);
}
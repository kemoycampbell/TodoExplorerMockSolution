using TodoExploerProject.Models;

namespace TodoExploerProject.Services;

public interface ITodoService
{
    public Task<TodoResponse> GetTodos(ITodoFilter filter);
    public Task<TodoResponse> GetTodoByUserId(int userId);
    public Task<TodoResponse> GetTodoByCompleted(bool completed);
    public Task<TodoResponse> GetTodoByPartialTodo(string partialTodo);
    
}
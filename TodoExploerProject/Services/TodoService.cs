using Newtonsoft.Json;
using TodoExploerProject.Models;

namespace TodoExploerProject.Services;

public class TodoService: ITodoService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = "https://dummyjson.com/todos";
    
    
    public TodoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }


    public async Task<TodoResponse> GetTodos(ITodoFilter filter)
    {
        String uri = $"{_baseUrl}";
        
        //do we need to add /user?
        if (filter.UserId is not null)
        {
            uri += $"/user/{filter.UserId}";
        }
        
        //set limit to 0 since we want everything
        uri += "?limit=0";
        
        
        //make the requeswt
        HttpResponseMessage response = await _httpClient.GetAsync(uri);
        response.EnsureSuccessStatusCode();
        String responseBody = await response.Content.ReadAsStringAsync();
        
        TodoResponse  todoResponse = JsonConvert.DeserializeObject<TodoResponse>(responseBody);
        
        //applying filter for SearchTerm and Completed if not null since the API dont have parameters for those
        List<TodoModel> todos = todoResponse.Todos;
        
        //if we have a search term, we will filter out the search term from the results using LINQ
        if (filter.SearchTerm is not null)
        {
            todos = todos.Where(t => t.Todo.Contains(filter.SearchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        
        //if we have a completed filter, we will filter out the completed todos from the results using LINQ
        if (filter.Completed is not null)
        {
            //update the todos list to only include todos that match the completed filter
            todos = todos.Where(t => t.Completed == filter.Completed).ToList();
        }
        
        //update the todoResponse with the filtered todos and the total count
        todoResponse.Todos = todos;
        todoResponse.Total = todos.Count;
        
        
        return todoResponse;
        
    }

    public async Task<TodoResponse> GetTodoByUserId(int userId)
    {
        //build the filter matching the ITodoFilter interface
        ITodoFilter filter = new TodoFilter()
        {
            UserId = userId
        };
        
        return await GetTodos(filter);
    }

    public async Task<TodoResponse> GetTodoByCompleted(bool completed)
    {
        ITodoFilter filter = new TodoFilter()
        {
            Completed = completed
        };
        
        return await GetTodos(filter);
    }

    public async Task<TodoResponse> GetTodoByPartialTodo(string partialTodo)
    {
        ITodoFilter filter = new TodoFilter()
        {
            SearchTerm = partialTodo
        };
        return await GetTodos(filter);
    }
}
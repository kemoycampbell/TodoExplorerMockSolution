using System.Net;
using System.Text;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using TodoExploerProject.Models;
using TodoExploerProject.Services;

namespace Tests;

public class TodoServiceTests : IDisposable
{
    private readonly Mock<HttpMessageHandler> handlerMock;
    private readonly HttpClient httpClient;
    private readonly TodoService todoService;

    public TodoServiceTests()
    {
        handlerMock = new Mock<HttpMessageHandler>();

        httpClient = new HttpClient(handlerMock.Object);

        todoService = new TodoService(httpClient);
    }

    private void SetupHttpResponse(
        List<TodoModel> todos,
        HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        TodoResponse response = new TodoResponse
        {
            Todos = todos,
            Total = todos.Count
        };

        string json = JsonConvert.SerializeObject(response);

        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json")
            });
    }

    private List<TodoModel> GetSampleTodos()
    {
        return new List<TodoModel>
        {
            new TodoModel
            {
                Id = 1,
                Todo = "Learn C#",
                Completed = false,
                UserId = 5
            },

            new TodoModel
            {
                Id = 2,
                Todo = "Learn MAUI",
                Completed = true,
                UserId = 5
            },

            new TodoModel
            {
                Id = 3,
                Todo = "Learn Python",
                Completed = false,
                UserId = 10
            },

            new TodoModel
            {
                Id = 4,
                Todo = "Build an application",
                Completed = true,
                UserId = 10
            }
        };
    }

    [Fact]
    public async Task GetTodos_ReturnsAllTodos()
    {
        // Arrange
        SetupHttpResponse(GetSampleTodos());

        ITodoFilter filter = new TodoFilter();

        // Act
        TodoResponse result = await todoService.GetTodos(filter);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(4, result.Todos.Count);
        Assert.Equal(4, result.Total);
    }

    [Fact]
    public async Task GetTodos_WithSearchTerm_ReturnsMatchingTodos()
    {
        // Arrange
        SetupHttpResponse(GetSampleTodos());

        ITodoFilter filter = new TodoFilter
        {
            SearchTerm = "learn"
        };

        // Act
        TodoResponse result = await todoService.GetTodos(filter);

        // Assert
        Assert.Equal(3, result.Todos.Count);

        Assert.All(result.Todos, todo =>
            Assert.Contains(
                "learn",
                todo.Todo,
                StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task GetTodos_WithCompletedFilter_ReturnsCompletedTodos()
    {
        // Arrange
        SetupHttpResponse(GetSampleTodos());

        ITodoFilter filter = new TodoFilter
        {
            Completed = true
        };

        // Act
        TodoResponse result = await todoService.GetTodos(filter);

        // Assert
        Assert.Equal(2, result.Todos.Count);

        Assert.All(result.Todos, todo =>
            Assert.True(todo.Completed));
    }

    [Fact]
    public async Task GetTodos_WithIncompleteFilter_ReturnsIncompleteTodos()
    {
        // Arrange
        SetupHttpResponse(GetSampleTodos());

        ITodoFilter filter = new TodoFilter
        {
            Completed = false
        };

        // Act
        TodoResponse result = await todoService.GetTodos(filter);

        // Assert
        Assert.Equal(2, result.Todos.Count);

        Assert.All(result.Todos, todo =>
            Assert.False(todo.Completed));
    }

    [Fact]
    public async Task GetTodos_WithMultipleFilters_ReturnsMatchingTodos()
    {
        // Arrange
        SetupHttpResponse(GetSampleTodos());

        ITodoFilter filter = new TodoFilter
        {
            SearchTerm = "learn",
            Completed = false
        };

        // Act
        TodoResponse result = await todoService.GetTodos(filter);

        // Assert
        Assert.Equal("Learn C#", result.Todos[0].Todo);
        Assert.False(result.Todos[0].Completed);
        Assert.Equal(2, result.Total);
    }

    [Fact]
    public async Task GetTodoByUserId_UsesCorrectApiEndpoint()
    {
        // Arrange
        SetupHttpResponse(GetSampleTodos());

        // Act
        await todoService.GetTodoByUserId(5);

        // Assert
        handlerMock.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(request =>
                request.RequestUri!.ToString() ==
                "https://dummyjson.com/todos/user/5?limit=0"),
            ItExpr.IsAny<CancellationToken>()
        );
    }

    [Fact]
    public async Task GetTodoByCompleted_ReturnsMatchingTodos()
    {
        // Arrange
        SetupHttpResponse(GetSampleTodos());

        // Act
        TodoResponse result =
            await todoService.GetTodoByCompleted(true);

        // Assert
        Assert.Equal(2, result.Todos.Count);

        Assert.All(result.Todos, todo =>
            Assert.True(todo.Completed));
    }

    [Fact]
    public async Task GetTodoByPartialTodo_ReturnsMatchingTodos()
    {
        // Arrange
        SetupHttpResponse(GetSampleTodos());

        // Act
        TodoResponse result =
            await todoService.GetTodoByPartialTodo("Python");

        // Assert
        Assert.Single(result.Todos);

        Assert.Equal("Learn Python", result.Todos[0].Todo);
    }

    [Fact]
    public async Task GetTodos_WhenApiFails_ThrowsHttpRequestException()
    {
        // Arrange
        SetupHttpResponse(
            GetSampleTodos(),
            HttpStatusCode.InternalServerError);

        ITodoFilter filter = new TodoFilter();

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(
            () => todoService.GetTodos(filter));
    }

    public void Dispose()
    {
        httpClient.Dispose();
    }
}
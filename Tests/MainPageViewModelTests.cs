
using System.Collections.ObjectModel;
using Moq;
using TodoExploerProject.Models;
using TodoExploerProject.Services;
using TodoExploerProject.ViewModels;

namespace Tests;

public class MainPageViewModelTests
{
    private readonly Mock<ITodoService> todoServiceMock;
    private readonly Mock<IFavoriteStorage> storageMock;
    private readonly Mock<IFavoriteViewModel> favoriteViewModelMock;
    private readonly Mock<INavigationService> navigationServiceMock;
    private readonly Mock<ITodoDetailViewModel> todoDetailViewModelMock;

    private readonly MainPageViewModel viewModel;

    public MainPageViewModelTests()
    {
        todoServiceMock = new Mock<ITodoService>();
        storageMock = new Mock<IFavoriteStorage>();
        favoriteViewModelMock = new Mock<IFavoriteViewModel>();
        navigationServiceMock = new Mock<INavigationService>();
        todoDetailViewModelMock = new Mock<ITodoDetailViewModel>();

        viewModel = new MainPageViewModel(
            todoServiceMock.Object,
            new ObservableCollection<TodoModel>(),
            storageMock.Object,
            favoriteViewModelMock.Object,
            navigationServiceMock.Object,
            todoDetailViewModelMock.Object
        );
    }

    // --------------------------------------------------
    // LoadTodos Tests
    // --------------------------------------------------

    [Fact]
    public async Task LoadTodos_WhenCalled_LoadsTodosFromService()
    {
        // Arrange
        List<TodoModel> todos = new List<TodoModel>
        {
            new TodoModel { Id = 1, Todo = "Learn C#", Completed = false },
            new TodoModel { Id = 2, Todo = "Learn MAUI", Completed = true }
        };

        todoServiceMock
            .Setup(x => x.GetTodos(It.IsAny<TodoFilter>()))
            .ReturnsAsync(new TodoResponse { Todos = todos });

        // Act
        await viewModel.LoadTodos();

        // Assert
        Assert.Equal(2, viewModel.Todos.Count);
        Assert.Equal("Learn C#", viewModel.Todos[0].Todo);
        Assert.Equal("Learn MAUI", viewModel.Todos[1].Todo);

        todoServiceMock.Verify(
            x => x.GetTodos(It.IsAny<TodoFilter>()),
            Times.Once);
    }

    [Fact]
    public async Task LoadTodos_WhenCalled_UsesDefaultFilter()
    {
        // Arrange
        todoServiceMock
            .Setup(x => x.GetTodos(It.IsAny<TodoFilter>()))
            .ReturnsAsync(new TodoResponse
            {
                Todos = new List<TodoModel>()
            });

        // Act
        await viewModel.LoadTodos();

        // Assert
        todoServiceMock.Verify(
            x => x.GetTodos(It.Is<TodoFilter>(filter =>
                filter.SearchTerm == null &&
                filter.Completed == null &&
                filter.UserId == null)),
            Times.Once);
    }

    [Fact]
    public async Task LoadTodos_WhenCompleted_SetsIsLoadingToFalse()
    {
        // Arrange
        todoServiceMock
            .Setup(x => x.GetTodos(It.IsAny<TodoFilter>()))
            .ReturnsAsync(new TodoResponse
            {
                Todos = new List<TodoModel>()
            });

        // Act
        await viewModel.LoadTodos();

        // Assert
        Assert.False(viewModel.IsLoading);
    }

    // --------------------------------------------------
    // ToggleFavorite Tests
    // --------------------------------------------------

    [Fact]
    public async Task ToggleFavorite_WhenTodoIsNotFavorite_SetsFavoriteToTrue()
    {
        // Arrange
        TodoModel todo = new TodoModel
        {
            Id = 1,
            Todo = "Learn C#",
            IsFavorite = false
        };

        // Act
        await viewModel.ToggleFavoriteCommand.ExecuteAsync(todo);

        // Assert
        Assert.True(todo.IsFavorite);
    }

    [Fact]
    public async Task ToggleFavorite_WhenTodoIsFavorite_SetsFavoriteToFalse()
    {
        // Arrange
        TodoModel todo = new TodoModel
        {
            Id = 1,
            Todo = "Learn C#",
            IsFavorite = true
        };

        // Act
        await viewModel.ToggleFavoriteCommand.ExecuteAsync(todo);

        // Assert
        Assert.False(todo.IsFavorite);
    }

    [Fact]
    public async Task ToggleFavorite_WhenCalled_SavesFavoriteTodos()
    {
        // Arrange
        TodoModel favoriteTodo = new TodoModel
        {
            Id = 1,
            Todo = "Learn C#",
            IsFavorite = false
        };

        TodoModel regularTodo = new TodoModel
        {
            Id = 2,
            Todo = "Learn MAUI",
            IsFavorite = false
        };

        viewModel.Todos.Add(favoriteTodo);
        viewModel.Todos.Add(regularTodo);

        // Act
        await viewModel.ToggleFavoriteCommand.ExecuteAsync(favoriteTodo);

        // Assert
        storageMock.Verify(
            x => x.SaveFavorites(It.Is<List<TodoModel>>(favorites =>
                favorites.Count == 1 &&
                favorites.Contains(favoriteTodo))),
            Times.Once);
    }

    [Fact]
    public async Task ToggleFavorite_WhenTodoIsNull_DoesNotSaveFavorites()
    {
        // Act
        await viewModel.ToggleFavoriteCommand.ExecuteAsync(null);

        // Assert
        storageMock.Verify(
            x => x.SaveFavorites(It.IsAny<List<TodoModel>>()),
            Times.Never);
    }

    // --------------------------------------------------
    // ShowDetails Tests
    // --------------------------------------------------

    [Fact]
    public async Task ShowDetails_WhenCalled_SetsSelectedTodo()
    {
        // Arrange
        TodoModel todo = new TodoModel
        {
            Id = 1,
            Todo = "Learn C#"
        };

        // Act
        await viewModel.ShowDetailsCommand.ExecuteAsync(todo);

        // Assert
        todoDetailViewModelMock.VerifySet(
            x => x.SelectedTodo = todo,
            Times.Once);
    }

    [Fact]
    public async Task ShowDetails_WhenCalled_NavigatesToDetailPage()
    {
        // Arrange
        TodoModel todo = new TodoModel
        {
            Id = 1,
            Todo = "Learn C#"
        };

        // Act
        await viewModel.ShowDetailsCommand.ExecuteAsync(todo);

        // Assert
        navigationServiceMock.Verify(
            x => x.GoToToDoDetail(todoDetailViewModelMock.Object),
            Times.Once);
    }

    [Fact]
    public async Task ShowDetails_WhenTodoIsNull_DoesNotNavigate()
    {
        // Act
        await viewModel.ShowDetailsCommand.ExecuteAsync(null);

        // Assert
        navigationServiceMock.Verify(
            x => x.GoToToDoDetail(It.IsAny<ITodoDetailViewModel>()),
            Times.Never);
    }

    // --------------------------------------------------
    // FavoritesNavPage Tests
    // --------------------------------------------------

    [Fact]
    public async Task FavoritesNavPage_WhenCalled_LoadsFavorites()
    {
        // Arrange
        List<TodoModel> favorites = new List<TodoModel>
        {
            new TodoModel { Id = 1, Todo = "Learn C#", IsFavorite = true },
            new TodoModel { Id = 2, Todo = "Learn MAUI", IsFavorite = true }
        };

        storageMock
            .Setup(x => x.GetFavorites())
            .ReturnsAsync(favorites);

        // Act
        await viewModel.FavoritesNavPageCommand.ExecuteAsync(null);

        // Assert
        favoriteViewModelMock.VerifySet(
            x => x.Todos = It.Is<ObservableCollection<TodoModel>>(
                todos => todos.Count == 2),
            Times.Once);
    }

    [Fact]
    public async Task FavoritesNavPage_WhenCalled_NavigatesToFavorites()
    {
        // Arrange
        storageMock
            .Setup(x => x.GetFavorites())
            .ReturnsAsync(new List<TodoModel>());

        // Act
        await viewModel.FavoritesNavPageCommand.ExecuteAsync(null);

        // Assert
        navigationServiceMock.Verify(
            x => x.GoToFavorites(favoriteViewModelMock.Object),
            Times.Once);
    }

    // --------------------------------------------------
    // ReloadTodos Tests
    // --------------------------------------------------

    [Fact]
    public async Task ReloadTodos_WhenCalled_ClearsExistingTodosAndReloads()
    {
        // Arrange
        viewModel.Todos.Add(new TodoModel
        {
            Id = 1,
            Todo = "Old Todo"
        });

        List<TodoModel> newTodos = new List<TodoModel>
        {
            new TodoModel { Id = 2, Todo = "New Todo" }
        };

        todoServiceMock
            .Setup(x => x.GetTodos(It.IsAny<TodoFilter>()))
            .ReturnsAsync(new TodoResponse { Todos = newTodos });

        // Act
        await viewModel.ReloadTodosCommand.ExecuteAsync(null);

        // Assert
        Assert.Single(viewModel.Todos);
        Assert.Equal("New Todo", viewModel.Todos[0].Todo);
    }

    // --------------------------------------------------
    // FilterTodos Tests
    // --------------------------------------------------

    [Fact]
    public async Task FilterTodos_WhenSearchTextIsProvided_UsesSearchFilter()
    {
        // Arrange
        viewModel.SearchText = "Learn";

        todoServiceMock
            .Setup(x => x.GetTodos(It.IsAny<TodoFilter>()))
            .ReturnsAsync(new TodoResponse
            {
                Todos = new List<TodoModel>()
            });

        // Act
        await viewModel.FilterTodosCommand.ExecuteAsync(null);

        // Assert
        todoServiceMock.Verify(
            x => x.GetTodos(It.Is<TodoFilter>(filter =>
                filter.SearchTerm == "Learn")),
            Times.Once);
    }

    [Fact]
    public async Task FilterTodos_WhenCompletionFilterIsCompleted_UsesCompletedFilter()
    {
        // Arrange
        viewModel.CompletionFilter = "Completed";

        todoServiceMock
            .Setup(x => x.GetTodos(It.IsAny<TodoFilter>()))
            .ReturnsAsync(new TodoResponse
            {
                Todos = new List<TodoModel>()
            });

        // Act
        await viewModel.FilterTodosCommand.ExecuteAsync(null);

        // Assert
        todoServiceMock.Verify(
            x => x.GetTodos(It.Is<TodoFilter>(filter =>
                filter.Completed == true)),
            Times.Once);
    }

    [Fact]
    public async Task FilterTodos_WhenCompletionFilterIsIncompleted_UsesIncompletedFilter()
    {
        // Arrange
        viewModel.CompletionFilter = "Incompleted";

        todoServiceMock
            .Setup(x => x.GetTodos(It.IsAny<TodoFilter>()))
            .ReturnsAsync(new TodoResponse
            {
                Todos = new List<TodoModel>()
            });

        // Act
        await viewModel.FilterTodosCommand.ExecuteAsync(null);

        // Assert
        todoServiceMock.Verify(
            x => x.GetTodos(It.Is<TodoFilter>(filter =>
                filter.Completed == false)),
            Times.Once);
    }

    [Fact]
    public async Task FilterTodos_WhenUserIdIsProvided_UsesUserIdFilter()
    {
        // Arrange
        viewModel.SearchUserId = 5;

        todoServiceMock
            .Setup(x => x.GetTodos(It.IsAny<TodoFilter>()))
            .ReturnsAsync(new TodoResponse
            {
                Todos = new List<TodoModel>()
            });

        // Act
        await viewModel.FilterTodosCommand.ExecuteAsync(null);

        // Assert
        todoServiceMock.Verify(
            x => x.GetTodos(It.Is<TodoFilter>(filter =>
                filter.UserId == 5)),
            Times.Once);
    }

    [Fact]
    public async Task FilterTodos_WhenMultipleFiltersProvided_UsesAllFilters()
    {
        // Arrange
        viewModel.SearchText = "Learn";
        viewModel.CompletionFilter = "Completed";
        viewModel.SearchUserId = 5;

        todoServiceMock
            .Setup(x => x.GetTodos(It.IsAny<TodoFilter>()))
            .ReturnsAsync(new TodoResponse
            {
                Todos = new List<TodoModel>()
            });

        // Act
        await viewModel.FilterTodosCommand.ExecuteAsync(null);

        // Assert
        todoServiceMock.Verify(
            x => x.GetTodos(It.Is<TodoFilter>(filter =>
                filter.SearchTerm == "Learn" &&
                filter.Completed == true &&
                filter.UserId == 5)),
            Times.Once);
    }

    [Fact]
    public async Task FilterTodos_WhenCalled_UpdatesTodosCollection()
    {
        // Arrange
        List<TodoModel> filteredTodos = new List<TodoModel>
        {
            new TodoModel { Id = 10, Todo = "Filtered Todo" }
        };

        todoServiceMock
            .Setup(x => x.GetTodos(It.IsAny<TodoFilter>()))
            .ReturnsAsync(new TodoResponse { Todos = filteredTodos });

        // Act
        await viewModel.FilterTodosCommand.ExecuteAsync(null);

        // Assert
        Assert.Single(viewModel.Todos);
        Assert.Equal("Filtered Todo", viewModel.Todos[0].Todo);
    }

    [Fact]
    public async Task FilterTodos_WhenCalled_SetsIsLoadingToFalse()
    {
        // Arrange
        todoServiceMock
            .Setup(x => x.GetTodos(It.IsAny<TodoFilter>()))
            .ReturnsAsync(new TodoResponse
            {
                Todos = new List<TodoModel>()
            });

        // Act
        await viewModel.FilterTodosCommand.ExecuteAsync(null);

        // Assert
        Assert.False(viewModel.IsLoading);
    }
}

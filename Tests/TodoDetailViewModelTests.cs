using Moq;
using TodoExploerProject.Models;
using TodoExploerProject.Services;
using TodoExploerProject.ViewModels;

namespace Tests;

public class TodoDetailViewModelTests
{
    private readonly Mock<INavigationService> navigationServiceMock;
    private readonly TodoDetailViewModel viewModel;

    public TodoDetailViewModelTests()
    {
        navigationServiceMock = new Mock<INavigationService>();

        navigationServiceMock
            .Setup(x => x.PopAsync())
            .Returns(Task.CompletedTask);

        viewModel = new TodoDetailViewModel(
            navigationServiceMock.Object);
    }

    [Fact]
    public void SelectedTodo_WhenAssigned_ReturnsCorrectTodo()
    {
        // Arrange
        TodoModel todo = new TodoModel
        {
            Id = 1,
            Todo = "Learn C#",
            Completed = false,
            UserId = 5,
            IsFavorite = true
        };

        // Act
        viewModel.SelectedTodo = todo;

        // Assert
        Assert.NotNull(viewModel.SelectedTodo);
        Assert.Same(todo, viewModel.SelectedTodo);
        Assert.Equal(1, viewModel.SelectedTodo.Id);
        Assert.Equal("Learn C#", viewModel.SelectedTodo.Todo);
    }

    [Fact]
    public async Task GoBackToMainPageCommand_Executed_CallsPopAsync()
    {
        // Arrange

        // Act
        await viewModel.GoBackToMainPageCommand.ExecuteAsync(null);

        // Assert
        navigationServiceMock.Verify(
            x => x.PopAsync(),
            Times.Once);
    }

    [Fact]
    public async Task GoBackToMainPageCommand_Executed_CallsPopAsyncOnlyOnce()
    {
        // Arrange

        // Act
        await viewModel.GoBackToMainPageCommand.ExecuteAsync(null);

        // Assert
        navigationServiceMock.Verify(
            x => x.PopAsync(),
            Times.Once);
    }
}
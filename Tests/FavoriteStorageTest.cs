using TodoExploerProject.Models;
using TodoExploerProject.Services;

namespace Tests;

public class FavoriteStorageTest
{
    private readonly string directory;
    private readonly string filename = "favorites.json";
    private readonly IFavoriteStorage storage;

    public FavoriteStorageTest()
    {
        // Create a temporary directory for each test
        directory = Path.Combine(
            Path.GetTempPath(),
            Guid.NewGuid().ToString());

        Directory.CreateDirectory(directory);

        storage = new FavoriteStorage(directory, filename);
    }
    
        [Fact]
    public async Task GetFavorites_FileDoesNotExist_ReturnsEmptyList()
    {
        // Act
        List<TodoModel> result = await storage.GetFavorites();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task SaveFavorites_CreatesFile()
    {
        // Arrange
        List<TodoModel> favorites = new()
        {
            new TodoModel
            {
                Id = 1,
                Todo = "Learn C#",
                Completed = false,
                UserId = 5,
                IsFavorite = true
            }
        };

        // Act
        await storage.SaveFavorites(favorites);

        // Assert
        string path = Path.Combine(directory, filename);

        Assert.True(File.Exists(path));
    }

    [Fact]
    public async Task GetFavorites_FileExists_ReturnsSavedFavorites()
    {
        // Arrange
        List<TodoModel> favorites = new()
        {
            new TodoModel
            {
                Id = 1,
                Todo = "Learn C#",
                Completed = false,
                UserId = 5,
                IsFavorite = true
            }
        };

        await storage.SaveFavorites(favorites);

        // Act
        List<TodoModel> result = await storage.GetFavorites();

        // Assert
        Assert.Single(result);

        Assert.Equal(favorites[0].Id, result[0].Id);
        Assert.Equal(favorites[0].Todo, result[0].Todo);
        Assert.Equal(favorites[0].Completed, result[0].Completed);
        Assert.Equal(favorites[0].UserId, result[0].UserId);
        Assert.Equal(favorites[0].IsFavorite, result[0].IsFavorite);
    }

    [Fact]
    public async Task SaveFavorites_ExistingFile_OverwritesPreviousData()
    {
        // Arrange
        List<TodoModel> firstFavorites = new()
        {
            new TodoModel
            {
                Id = 1,
                Todo = "Learn C#",
                Completed = false,
                UserId = 5,
                IsFavorite = true
            }
        };

        List<TodoModel> secondFavorites = new()
        {
            new TodoModel
            {
                Id = 2,
                Todo = "Learn MAUI",
                Completed = true,
                UserId = 10,
                IsFavorite = true
            }
        };

        await storage.SaveFavorites(firstFavorites);

        // Act
        await storage.SaveFavorites(secondFavorites);

        List<TodoModel> result = await storage.GetFavorites();

        // Assert
        Assert.Single(result);
        Assert.Equal(2, result[0].Id);
        Assert.Equal("Learn MAUI", result[0].Todo);
    }

    public void Dispose()
    {
        // Clean up temporary files after each test
        if (Directory.Exists(directory))
        {
            Directory.Delete(directory, true);
        }
    }
}
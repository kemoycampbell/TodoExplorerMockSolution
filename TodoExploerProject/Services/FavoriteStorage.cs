using Newtonsoft.Json;
using TodoExploerProject.Models;

namespace TodoExploerProject.Services;

public class FavoriteStorage: IFavoriteStorage
{
    private string filePath;

    public FavoriteStorage(string directory, string filename)
    {
        this.filePath = Path.Combine(directory, filename);
    }

    public async Task<List<TodoModel>> GetFavorites()
    {
        //if nothing exist we return a empty list
        if (!File.Exists(this.filePath))
            return new List<TodoModel>();
        
        StreamReader reader = new StreamReader(this.filePath);
        string json = await reader.ReadToEndAsync();
        reader.Close();
        
        return JsonConvert.DeserializeObject<List<TodoModel>>(json);
    }

    public async Task SaveFavorites(List<TodoModel> favorites)
    {
        StreamWriter writer = new StreamWriter(filePath);
        string json = JsonConvert.SerializeObject(favorites);
        await writer.WriteAsync(json);
        writer.Close();
        
    }
}
using System.Collections.ObjectModel;
using Microsoft.Extensions.Logging;
using TodoExploerProject.Models;
using TodoExploerProject.Services;
using TodoExploerProject.ViewModels;

namespace TodoExploerProject;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<HttpClient>();
        builder.Services.AddSingleton<ObservableCollection<TodoModel>>();
        builder.Services.AddSingleton<ITodoService, TodoService>();
        builder.Services.AddTransient<IMainPageViewModel, MainPageViewModel>();
        builder.Services.AddTransient<MainPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}